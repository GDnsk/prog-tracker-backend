namespace ProgTracker.Application.Common;

public enum ResponseCode
{
    Ok = 200,
    Bad = 400,
}
    
public class Response<T>
{
    public int Code { get; private set; }
    public T Result { get; set; }

    public IEnumerable<Error> Errors { get; }

    public Response(int code = (int)ResponseCode.Ok)
    {
        Code = code;
    }
        
    public Response(T result, int code = (int)ResponseCode.Ok)
    {
        Code = code;
        Result = result;
    }
        
    public Response(IEnumerable<Error> errors, int code = (int)ResponseCode.Bad)
    {
        Code = code;
        Errors = errors;
    }
        
    public Response(Error error, int code = (int)ResponseCode.Bad)
    {
        Code = code;
        Errors = new [] { error };
    }
        

    public static Response<T> Bad(T value)
    {
        return new Response<T>(value, (int)ResponseCode.Bad);
    }
        
    public static Response<T> Bad()
    {
        return new Response<T>((int)ResponseCode.Bad);
    }
        
    public static Response<T> Ok(T value)
    {
        return new Response<T>(value);
    }
        
    public static Response<T> Ok()
    {
        return new Response<T>();
    }
}