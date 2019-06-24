using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Modules;

namespace ToolsTests.Modules
{
    [TestClass]
    public class GeneratorUtilsTests
    {
        [TestMethod]
        public void TestCamelCase()
        {
            Assert.AreEqual(GeneratorUtils.CamelCase("field name"), "FieldName");
            Assert.AreEqual(GeneratorUtils.CamelCase("another field_name"), "AnotherFieldName");
            Assert.AreEqual(GeneratorUtils.CamelCase("another field_name"), "AnotherFieldName");
            Assert.AreEqual(GeneratorUtils.CamelCase("another fieldName"), "AnotherFieldName");
        }

        [TestMethod]
        public void TestSingularize()
        {
            Assert.AreEqual(GeneratorUtils.Singularize("files"), "file");
            Assert.AreEqual(GeneratorUtils.Singularize("centres"), "centre");
            Assert.AreEqual(GeneratorUtils.Singularize("girls"), "girl");
            Assert.AreEqual(GeneratorUtils.Singularize("books"), "book");
            Assert.AreEqual(GeneratorUtils.Singularize("computers"), "computer");
            Assert.AreEqual(GeneratorUtils.Singularize("ambitions"), "ambition");

            Assert.AreEqual(GeneratorUtils.Singularize("washes"), "wash");
            Assert.AreEqual(GeneratorUtils.Singularize("boxes"), "box");
            Assert.AreEqual(GeneratorUtils.Singularize("matches"), "match");
            Assert.AreEqual(GeneratorUtils.Singularize("glasses"), "glass");
            Assert.AreEqual(GeneratorUtils.Singularize("buses"), "bus");
            Assert.AreEqual(GeneratorUtils.Singularize("businesses"), "business");

            Assert.AreEqual(GeneratorUtils.Singularize("knives"), "knife");
            Assert.AreEqual(GeneratorUtils.Singularize("lives"), "life");
            Assert.AreEqual(GeneratorUtils.Singularize("wives"), "wife");
            Assert.AreEqual(GeneratorUtils.Singularize("shelves"), "shelf");

            Assert.AreEqual(GeneratorUtils.Singularize("cliffs"), "cliff");
            Assert.AreEqual(GeneratorUtils.Singularize("sniffs"), "sniff");
            Assert.AreEqual(GeneratorUtils.Singularize("scoffs"), "scoff");
            Assert.AreEqual(GeneratorUtils.Singularize("toffs"), "toff");
            Assert.AreEqual(GeneratorUtils.Singularize("stiffs"), "stiff");
            Assert.AreEqual(GeneratorUtils.Singularize("tiffs"), "tiff");

            Assert.AreEqual(GeneratorUtils.Singularize("boys"), "boy");
            Assert.AreEqual(GeneratorUtils.Singularize("journeys"), "journey");
            Assert.AreEqual(GeneratorUtils.Singularize("keys"), "key");
            Assert.AreEqual(GeneratorUtils.Singularize("trays"), "tray");

            Assert.AreEqual(GeneratorUtils.Singularize("countries"), "country");
            Assert.AreEqual(GeneratorUtils.Singularize("babies"), "baby");
            Assert.AreEqual(GeneratorUtils.Singularize("bodies"), "body");
            Assert.AreEqual(GeneratorUtils.Singularize("memories"), "memory");

            Assert.AreEqual(GeneratorUtils.Singularize("radios"), "radio");
            Assert.AreEqual(GeneratorUtils.Singularize("stereos"), "stereo");
            Assert.AreEqual(GeneratorUtils.Singularize("videos"), "video");

            Assert.AreEqual(GeneratorUtils.Singularize("heroes"), "hero");
            Assert.AreEqual(GeneratorUtils.Singularize("potatoes"), "potato");
            Assert.AreEqual(GeneratorUtils.Singularize("volcanoes"), "volcano");
            Assert.AreEqual(GeneratorUtils.Singularize("tomatoes"), "tomato");

            Assert.AreEqual(GeneratorUtils.Singularize("cars"), "car");
            Assert.AreEqual(GeneratorUtils.Singularize("pennies"), "penny");
            Assert.AreEqual(GeneratorUtils.Singularize("witnesses"), "witness");
            Assert.AreEqual(GeneratorUtils.Singularize("waitresses"), "waitress");

            Assert.AreEqual(GeneratorUtils.Singularize("addresses"), "address");
        }

        [TestMethod]
        public void TestPluralize()
        {
            Assert.AreEqual(GeneratorUtils.Pluralize("file"), "files");
            Assert.AreEqual(GeneratorUtils.Pluralize("centre"), "centres");
            Assert.AreEqual(GeneratorUtils.Pluralize("girl"), "girls");
            Assert.AreEqual(GeneratorUtils.Pluralize("book"), "books");
            Assert.AreEqual(GeneratorUtils.Pluralize("computer"), "computers");
            Assert.AreEqual(GeneratorUtils.Pluralize("ambition"), "ambitions");

            Assert.AreEqual(GeneratorUtils.Pluralize("wash"), "washes");
            Assert.AreEqual(GeneratorUtils.Pluralize("box"), "boxes");
            Assert.AreEqual(GeneratorUtils.Pluralize("match"), "matches");
            Assert.AreEqual(GeneratorUtils.Pluralize("glass"), "glasses");
            Assert.AreEqual(GeneratorUtils.Pluralize("bus"), "buses");
            Assert.AreEqual(GeneratorUtils.Pluralize("business"), "businesses");

            Assert.AreEqual(GeneratorUtils.Pluralize("knife"), "knives");
            Assert.AreEqual(GeneratorUtils.Pluralize("life"), "lives");
            Assert.AreEqual(GeneratorUtils.Pluralize("wife"), "wives");
            Assert.AreEqual(GeneratorUtils.Pluralize("shelf"), "shelves");

            Assert.AreEqual(GeneratorUtils.Pluralize("cliff"), "cliffs");
            Assert.AreEqual(GeneratorUtils.Pluralize("sniff"), "sniffs");
            Assert.AreEqual(GeneratorUtils.Pluralize("scoff"), "scoffs");
            Assert.AreEqual(GeneratorUtils.Pluralize("toff"), "toffs");
            Assert.AreEqual(GeneratorUtils.Pluralize("stiff"), "stiffs");
            Assert.AreEqual(GeneratorUtils.Pluralize("tiff"), "tiffs");

            Assert.AreEqual(GeneratorUtils.Pluralize("boy"), "boys");
            Assert.AreEqual(GeneratorUtils.Pluralize("journey"), "journeys");
            Assert.AreEqual(GeneratorUtils.Pluralize("key"), "keys");
            Assert.AreEqual(GeneratorUtils.Pluralize("tray"), "trays");

            Assert.AreEqual(GeneratorUtils.Pluralize("country"), "countries");
            Assert.AreEqual(GeneratorUtils.Pluralize("baby"), "babies");
            Assert.AreEqual(GeneratorUtils.Pluralize("body"), "bodies");
            Assert.AreEqual(GeneratorUtils.Pluralize("memory"), "memories");

            Assert.AreEqual(GeneratorUtils.Pluralize("radio"), "radios");
            Assert.AreEqual(GeneratorUtils.Pluralize("stereo"), "stereos");
            Assert.AreEqual(GeneratorUtils.Pluralize("video"), "videos");

            Assert.AreEqual(GeneratorUtils.Pluralize("hero"), "heroes");
            Assert.AreEqual(GeneratorUtils.Pluralize("potato"), "potatoes");
            Assert.AreEqual(GeneratorUtils.Pluralize("volcano"), "volcanoes");
            Assert.AreEqual(GeneratorUtils.Pluralize("tomato"), "tomatoes");

            Assert.AreEqual(GeneratorUtils.Pluralize("car"), "cars");
            Assert.AreEqual(GeneratorUtils.Pluralize("penny"), "pennies");
            Assert.AreEqual(GeneratorUtils.Pluralize("witness"), "witnesses");
            Assert.AreEqual(GeneratorUtils.Pluralize("waitress"), "waitresses");

            Assert.AreEqual(GeneratorUtils.Pluralize("address"), "addresses");
        }

        [TestMethod]
        public void TestIncrementNumberedString()
        {
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("car"), "car1");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("volcano1"), "volcano2");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("penny3"), "penny4");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("witness9"), "witness10");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("waitress19"), "waitress20");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("potato20"), "potato21");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("country99"), "country100");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("journey0"), "journey1");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("tomato00"), "tomato01");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("glass01"), "glass02");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("journey000"), "journey001");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("bus09"), "bus010");
            Assert.AreEqual(GeneratorUtils.IncrementNumberedString("box089"), "box090");
        }

        [TestMethod]
        public void TestToBoolean()
        {
            Assert.AreEqual(GeneratorUtils.ToBoolean(0), false);
            Assert.AreEqual(GeneratorUtils.ToBoolean(1), true);
            Assert.AreEqual(GeneratorUtils.ToBoolean(5), true);
            Assert.AreEqual(GeneratorUtils.ToBoolean(-1), false);
        }

    }
}