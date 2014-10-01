using Models.DataContexts;

namespace BLL
{
    public abstract class AppContextCreator
    {
        public abstract IAppContext Create();
    }

    public class ConcreateAppContextCreator : AppContextCreator
    {
        private static IAppContext context;

        public override IAppContext Create()
        {
            return context ?? (context = new MyAppContext());
        }
    }
}
