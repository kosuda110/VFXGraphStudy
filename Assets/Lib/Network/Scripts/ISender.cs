
namespace Kosu.UnityLibrary
{
    public interface ISender
    {
        void Send<T>(T data) where T : class;
        void Close();
    }
}
