﻿using System.Diagnostics.CodeAnalysis;
using Enterprise.Domain.Validation;

namespace Enterprise.Domain.Generic
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result can not be accessed.");

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
