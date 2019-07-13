using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Gender : IdentifiableItem
    {
        private GenderType genderType;
        [XmlIgnore]
        public GenderType GenderType { get => genderType; set => SetField(ref genderType, value); }
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
        public BindingList<GenderExpression> GenderExpressions { get; } = new BindingList<GenderExpression>();

        [XmlArray]
        [XmlArrayItem("Sex")]
        public BindingList<Sex> Sexes { get; } = new BindingList<Sex>();

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
