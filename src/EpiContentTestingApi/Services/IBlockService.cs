using System.Text;
using System.Web.UI.WebControls;
using EpiControlTestingApi.Common;

namespace EpiContent.Api.Services
{
    public interface IBlockService
    {
        BlockDto Add(BlockDto block);
    }

    public static class StringExtensions
    {
        public static string AddSpancesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var newText = new StringBuilder(text.Length*2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])))
                    {
                        newText.Append(' ');
                    }
                }

                newText.Append(text[i]);
            }
            {
                
            }
        }
    }
}