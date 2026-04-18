namespace CSDB_UtopiaModel.Model;

public class Navigator : IEnumerator<INavigable>
{
    public INavigable Current => throw new NotImplementedException();
    object System.Collections.IEnumerator.Current => Current; // explicit interface implementation, not to be modified
    public bool MoveNext() => throw new NotImplementedException();
    public void Reset() => throw new NotImplementedException();
    public void Dispose() => throw new NotImplementedException();
}