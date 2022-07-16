using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NewsApp.ViewModels
{
    public static class StaticMethods
    {


        public static string LoaiDau(string input)
        {
            string str = Regex.Replace(input, @"[^\w\d\s]", "");
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty)
                        .Replace('đ', 'd')
                        .Replace('Đ', 'D')
                        .Replace(" ", "-");
        }

        public static string DisplayImage(string imageURL)
        {
            if (!imageURL.StartsWith("http:"))
            {

                return imageURL.Replace(Directory.GetCurrentDirectory() + @"\wwwroot", "");
            }
            return imageURL;
        }


    }
}
