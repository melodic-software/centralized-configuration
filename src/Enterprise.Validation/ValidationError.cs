namespace Enterprise.Validation;

public record ValidationError(string PropertyName, string ErrorMessage);