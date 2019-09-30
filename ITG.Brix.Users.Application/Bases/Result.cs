using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITG.Brix.Users.Application.Bases
{
    public class Result
    {
        public int? Version { get; }
        public bool IsFailure { get; }
        public List<Failure> Failures { get; }

        public Result(bool hasFailures, List<Failure> failures)
        {
            if (!hasFailures && failures.Any())
            {
                throw new InvalidOperationException();
            }
            if (hasFailures && !failures.Any())
            {
                throw new InvalidOperationException();
            }

            IsFailure = hasFailures;
            Failures = new List<Failure>(failures);
        }

        public Result(int version, bool hasFailures, List<Failure> failures) : this(hasFailures, failures)
        {
            Version = version;
        }

        public static Result Fail(List<Failure> failures)
        {
            return new Result(true, failures);
        }

        public static Result<T> Fail<T>(List<Failure> failures)
        {
            return new Result<T>(default(T), true, failures);
        }

        public static Result Fail(string failure)
        {
            var errors = new List<Failure>
            {
                new CustomFault
                {
                    Message = failure
                }
            };
            return Result.Fail(errors);
        }

        public static Result Ok()
        {
            return new Result(false, new List<Failure>());
        }

        public static Result Ok(int version)
        {
            return new Result(version, false, new List<Failure>());
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, false, new List<Failure>());
        }

        public static Result<T> Ok<T>(T value, int version)
        {
            return new Result<T>(value, version, false, new List<Failure>());
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
        }

        [JsonConstructor]
        protected internal Result(T value, bool hasFailures, List<Failure> failures) : base(hasFailures, failures)
        {
            _value = value;
        }

        //[JsonConstructor]
        protected internal Result(T value, int version, bool hasFailures, List<Failure> failures) : base(version, hasFailures, failures)
        {
            _value = value;
        }

    }
}
