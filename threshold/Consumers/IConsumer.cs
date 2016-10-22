namespace threshold.Consumers
{
    public interface IConsumer<T>
    {
        string Name { get; }

        int ConsumeInterval { get; set; }

        void Start();

        void Stop();
    }
}
