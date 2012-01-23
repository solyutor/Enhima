using Enhima.Tests.Domain;
using NUnit.Framework;
using SharpTestsEx;

namespace Enhima.Tests
{
    [TestFixture]
    public class JoinedSubclassFixture : MappingFixture
    {
        [Test]
        public void Table_names_should_be_pluralized()
        {
            CompiledMappings.JoinedSubclasses.Satisfy(js =>
                                                      js.JoinedSubclass<JuristicPerson>().table == "JuristicPeople" &&
                                                      js.JoinedSubclass<NaturalPerson>().table == "NaturalPeople");
        }
        
        [Test]
        public void Key_column_should_be_named_Id()
        {
            CompiledMappings.JoinedSubclasses.Satisfy(js =>
                                                      js.JoinedSubclass<JuristicPerson>().key.column1 == "Id" &&
                                                      js.JoinedSubclass<NaturalPerson>().key.column1 == "Id");
        }
    }
}