namespace TT.Core.Domain.Entities;

public class Failure
{
	public string Message { get; private set; }

	public string Status { get; private set; }

	public Failure()
	{
		Message = string.Empty;
		Status = string.Empty;
	}

	public Failure(string message)
	{
		Message = message;
		Status = string.Empty;
	}

	public void SetMessage(string message)
	{
		Message = message;
	}

	public void SetStatus(string status)
	{
		Status = status;
	}
}
