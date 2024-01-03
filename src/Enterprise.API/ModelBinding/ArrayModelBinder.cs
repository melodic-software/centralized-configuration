using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Enterprise.API.ModelBinding;

// https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/custom-model-binding

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        ModelMetadata modelMetadata = bindingContext.ModelMetadata;
        bool isNotEnumerableType = !modelMetadata.IsEnumerableType;

        // Our binder currently only works on enumerable types.
        if (isNotEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        string modelName = bindingContext.ModelName;

        // Extract the inputted value through the value provider.
        IValueProvider valueProvider = bindingContext.ValueProvider;
        ValueProviderResult valueProviderResult = valueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        StringValues values = valueProviderResult.Values;
        string? firstValue = valueProviderResult.FirstValue;

        string valueString = valueProviderResult.ToString();

        if (string.IsNullOrEmpty(valueString))
        {
            // This is shared behavior by other built in model binders.
            // It is considered successful so other parts in the pipeline can react to this.
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        // Can we handle the model type?
        // TODO: Implement type checking.
        Type modelType = bindingContext.ModelType;

        // Now we know that the value isn't null or whitespace and the type of the model is enumerable.
        // We need to get the enumerable's type, and a converter (using reflection).

        Type elementType;

        if (modelMetadata.ElementType != null)
        {
            elementType = modelMetadata.ElementType;
        }
        else
        {
            TypeInfo modelTypeInfo = modelType.GetTypeInfo();
            elementType = modelTypeInfo.GenericTypeArguments[0];
        }

        TypeConverter converter = TypeDescriptor.GetConverter(elementType);
        bool isCommaSeparated = valueString.Contains(',');

        if (bindingContext.ModelMetadata.IsCollectionType)
        {
            // NOTE: This currently only works for List<T>.
            // TODO: Add factory pattern based off the type
            // We need to make sure that we're working with a List<T> before running the following code.
            // Structure this so we can add support for additional collection types down the road.

            // ALSO: If possible, refactor ALL this code into separate classes or methods that make this code easier to read and maintain.
            // It'd be nice if we could return early if it's not a concrete collection type.

            Type listType = typeof(List<>).MakeGenericType([elementType]);

            if (listType.IsAssignableFrom(modelType))
            {
                object? list = Activator.CreateInstance(listType);
                MethodInfo? addMethod = listType.GetMethod(nameof(List<object>.Add));

                if (addMethod == null)
                    return Task.CompletedTask;

                if (isCommaSeparated)
                {
                    string[] split = valueString.Split([","], StringSplitOptions.RemoveEmptyEntries);

                    foreach (string item in split)
                    {
                        string trimmedValue = item.Trim();
                        object? convertedValue = converter.ConvertFromString(trimmedValue);
                        addMethod.Invoke(list, [convertedValue]);
                    }
                }
                else
                {
                    addMethod.Invoke(list, [converter.ConvertFromString(valueString.Trim())]);
                }

                bindingContext.Model = list;
                bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
                return Task.CompletedTask;
            }
        }

        if (isCommaSeparated)
        {
            // Convert each item in the value list to the enumerable type.
            string[] split = valueString.Split([","], StringSplitOptions.RemoveEmptyEntries);
            object?[] converted = split.Select(x => converter.ConvertFromString(x.Trim())).ToArray();

            // Create an array of that type, and set it as the Model value.
            Array typedValues = Array.CreateInstance(elementType, converted.Length);
            converted.CopyTo(typedValues, 0); // Copy the object values to the GUID array.

            //TypeConverter modelTypeConverter = TypeDescriptor.GetConverter(modelMetadata.ModelType);
            //modelTypeConverter.ConvertFrom(typedValues);
            //object convertedTypeValues = Convert.ChangeType(typedValues, modelType);

            bindingContext.Model = typedValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        }
        else
        {
            throw new Exception("Format is not supported.");
        }

        return Task.CompletedTask;
    }
}