namespace SolaraNet.BusinessLogic.Contracts.Models
{
    /// <summary>
    /// Класс для генерации HTML текста на основе паттерна "Строитель"
    /// </summary>
    public class HtmlBuilder
    {
        private readonly string rootName;
        protected HtmlElement root = new();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public override string ToString() => root.ToString();

        public void Clear()
        {
            root = new HtmlElement { Name = rootName };
        }

        public HtmlElement Build() => root;

        public static implicit operator HtmlElement(HtmlBuilder builder)
        {
            return builder.root;
        }
    }
}