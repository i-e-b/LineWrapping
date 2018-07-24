namespace LineWrapping
{
    public interface ILineBreaker {
        /// <summary>
        /// Split a text into lines of less that the given width
        /// </summary>
        string[] BreakLines (string text, int lineWidth);
    }
}