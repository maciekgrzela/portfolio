namespace Application.Responses
{
    public class Response<T>
    {
        public bool Succeed { get; private init; }
        public ResponseResult Result { get; private init; }
        public string ErrorMessage { get; private init; }
        public T Value { get; private init; }

        public static Response<T> Success(ResponseResult result, T value) =>
            new() {Succeed = true, Result = result, Value = value};

        public static Response<T> Success(ResponseResult result) => 
            new() {Succeed = true, Result = result};

        public static Response<T> Failure(ResponseResult result, string message) =>
            new() {Succeed = false, Result = result, ErrorMessage = message};
    }
}