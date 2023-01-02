using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    public static class CommonValidators
    {
        public static bool EnumsContains<T>(string el, ICollection<string> accept = null, ICollection<string> remove = null) where T : Enum
        {
            var data = Enum.GetNames(typeof(T)).Select(x => x.ToLower());

            if (accept != null)
                data = data.Where(x => accept.Select(y => y.ToLower()).Contains(x));

            if (remove != null)
                data = data.Where(x => !remove.Select(y => y.ToLower()).Contains(x));

            return data.Contains(el.ToLower());
        }

        public static bool IsBase64String(string? base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        public static bool BeValidGuid(string? input)
        {
            Guid newGuid;
            var isValid = Guid.TryParse(input, out newGuid);

            if (newGuid == Guid.Empty)
                return false;

            return isValid;
        }

    }
}
