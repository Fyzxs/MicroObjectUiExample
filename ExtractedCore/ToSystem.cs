namespace ExtractedCore {
    public abstract class ToSystem<T>
    {
        public T ToSystemValue() => SystemValue();
        protected abstract T SystemValue();
    }
}