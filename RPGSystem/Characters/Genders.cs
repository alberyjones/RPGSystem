using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Gender : IdentifiableItem
    {
        [XmlIgnore]
        public GenderType GenderType { get; set; }
    }

    public class GenderExpression : Gender
    {
        public GenderExpression()
        {
            GenderType = GenderType.GenderExpression;
        }
    }

    public class Sex : Gender
    {
        public Sex()
        {
            GenderType = GenderType.Sex;
        }
    }

    public enum GenderType
    {
        Sex,
        GenderExpression
    }

    public class Genders : IdentifiableItemCollection<Gender>
    {
        [XmlArray]
        [XmlArrayItem("GenderExpression")]
        public List<GenderExpression> GenderExpressions { get; } = new List<GenderExpression>();

        [XmlArray]
        [XmlArrayItem("Sex")]
        public List<Sex> Sexes { get; } = new List<Sex>();

        protected override IEnumerable<Gender> EnumerateItems()
        {
            foreach (var item in GenderExpressions)
            {
                yield return item;
            }
            foreach (var item in Sexes)
            {
                yield return item;
            }
        }
    }
}
