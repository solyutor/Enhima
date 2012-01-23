namespace Enhima.Conventions
{
    public abstract class Convention
    {
        private readonly Mapper _mapper;

        protected Convention(Mapper mapper)
        {
            _mapper = mapper;
        }

        protected Mapper Mapper
        {
            get { return _mapper; }
        }

        public abstract void Attach();

        public abstract void Detach();
    }
}