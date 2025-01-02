using System;

namespace NoteBin
{
    public readonly struct Result<T, E>
    {
        private readonly T? value;
        private readonly E? error;
        private readonly bool ok;

        private Result(T? value, E? error, bool ok)
        {
            this.value = value;
            this.error = error;
            this.ok = ok;
        }

        public readonly bool IsOk => ok;
        public readonly bool IsErr => !ok;
        public readonly T Value => ok ? value! : throw new InvalidOperationException("Result does not contain an ok value!");
        public readonly E Error => !ok ? error! : throw new InvalidOperationException("Result does not contain an error value!");

        public static Result<T, E> Ok(T value) => new Result<T, E>(value, default, true);
        public static Result<T, E> Err(E value) => new Result<T, E>(default, value, false);
    }
}
