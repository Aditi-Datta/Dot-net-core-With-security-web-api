namespace webapisolution.Models
{
    public class MessageStatus
    {
        public object Data { get; set; } // Data related to the error (optional)
        public bool Status { get; set; } // Indicates the status of the operation
        public int Code { get; set; } // HTTP status code or custom error code
        public string Message { get; set; } // Error message
    }

}
