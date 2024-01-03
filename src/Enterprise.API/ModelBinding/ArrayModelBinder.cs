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

        // NOTE: This currently requires the model type to be an IEnumerable<T>.
        // If it is typed as a List<T>, it will blow up in the conversion / casting process either here or at the controller level.
        // TODO: Ensure these types of exceptions do not occur.
        // We could implement a model binder base and implement specific collection types
        // OR change this to a collection model binder and attempt to account for all the conversions.

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

        // For now, we're just returning if it's a concrete collection type.
        if (bindingContext.ModelMetadata.IsCollectionType)
            return Task.CompletedTask;

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