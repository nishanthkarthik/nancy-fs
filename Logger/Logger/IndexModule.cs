namespace Logger
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = (parameters) => View["index"];
            Post["/Log"] = (parameters) =>
            {

            };
        }
    }
}