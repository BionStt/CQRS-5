namespace CQRS.Models
{
    public class CQRSResponse
    {
        public int StatusCode { get; set; } = 200;
        public string ErrorMessage { get; set; }
        public bool IsSuccess => StatusCode == 200;
        public bool IsBadRequest => StatusCode == 400;
        public bool IsUnauthorised => StatusCode == 401;
        public bool IsUnsuccessful => !IsSuccess;
        public virtual bool HasData => false;
        public virtual object GetData() => null;

        public static CQRSResponse Success() => new CQRSResponse();
        public static CQRSResponse Bad(string message) => new CQRSResponse { StatusCode = 400, ErrorMessage = message };
        public static CQRSResponse Unauthorised => new CQRSResponse { StatusCode = 401 };
        public static CQRSResponse<T> Success<T>(T data) => new CQRSResponse<T> { Data = data };
        public static CQRSResponse NotFound(string message) => new CQRSResponse { StatusCode = 404, ErrorMessage = message };
    }

    public class CQRSResponse<T> : CQRSResponse
    {
        public T Data { get; set; }
        public override bool HasData => true;
        public override object GetData() => Data;
    }
}
