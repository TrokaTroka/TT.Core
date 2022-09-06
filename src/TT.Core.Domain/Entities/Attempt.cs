namespace TT.Core.Domain.Entities;

public struct Attempt<TFailure, TSuccess>
{
	public bool Succeeded { get; set; }

	public TFailure Failure { get; set; }

	public TSuccess Success { get; set; }

	public Attempt(TFailure failure)
	{
		Succeeded = false;
		Failure = failure;
		Success = default(TSuccess);
	}

	public Attempt(TSuccess success)
	{
		Succeeded = true;
		Failure = default(TFailure);
		Success = success;
	}

	public static implicit operator Attempt<TFailure, TSuccess>(TFailure failure)
	{
		return new Attempt<TFailure, TSuccess>(failure);
	}

	public static implicit operator Attempt<TFailure, TSuccess>(TSuccess success)
	{
		return new Attempt<TFailure, TSuccess>(success);
	}
}
