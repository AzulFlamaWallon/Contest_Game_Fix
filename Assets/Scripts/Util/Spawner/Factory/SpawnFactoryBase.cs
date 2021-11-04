public class SpawnFactoryBase<T> : ISpawnFactory<T> where T : new()
{
    public T Spawn()
    {
        return new T();
    }
}
