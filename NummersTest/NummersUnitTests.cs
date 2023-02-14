using Microsoft.VisualStudio.TestTools.UnitTesting;
using NummersApplicatie;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NummersTest
{
    [TestClass]
    public class NummersUnitTests
    {
        [TestMethod]
        public void TestSom()
        {
            var r = new Nummers();
            var sommethod = r.GetType().GetMethod("Som");
            if (sommethod != null)
            {
                int g1 = 5;
                int g2 = 6;
                SetGetallen(r, g1, g2);
                var res = sommethod.Invoke(r, null);
                Assert.AreEqual(typeof(int), sommethod.ReturnType, $"Verschil moet een int als returntype hebben , ik kreeg een {res.GetType()} terug");
                Assert.AreEqual(res, g1 + g2, $"Ik testte {g1}+{g2}  wat {g1 + g2} moest geven maar ik kreeg {res}");
            }
            else Assert.Fail("Som methode niet gevonden.");
        }


        [TestMethod]
        public void TestVerschil()
        {
            var r = new Nummers();
            var sommethod = r.GetType().GetMethod("Verschil");
            if (sommethod != null)
            {
                int g1 = 5;
                int g2 = 6;
                SetGetallen(r, g1, g2);
                var res = sommethod.Invoke(r, null);
                Assert.AreEqual(typeof(int), sommethod.ReturnType, $"Verschil moet een int als returntype hebben , ik kreeg een {res.GetType()} terug");
                Assert.AreEqual(res, g1 - g2, $"Ik testte {g1}-{g2}  wat {g1 - g2} moest geven maar ik kreeg {res}");
            }
            else Assert.Fail("Verschil methode niet gevonden.");
        }

        [TestMethod]
        public void TestProduct()
        {
            var r = new Nummers();
            var productMethode = r.GetType().GetMethod("Product");
            if (productMethode != null)
            {
                int g1 = 5;
                int g2 = 6;
                SetGetallen(r, g1, g2);
                var res = productMethode.Invoke(r, null);
                Assert.AreEqual(typeof(int), productMethode.ReturnType, $"Product moet een int als returntype hebben , ik kreeg een {res.GetType()} terug");
                Assert.AreEqual(res, g1 * g2, $"Ik testte {g1}*{g2}  wat {g1*g2} moest geven maar ik kreeg {res}");
            }
            else Assert.Fail("Product methode niet gevonden.");
        }


        [TestMethod]
        public void TestQuotient()
        {
            var r = new Nummers();
            var quotMethod = r.GetType().GetMethod("Quotient");
           

            if (quotMethod != null)
            {
                int g1 = 5;
                int g2 = 6;
                SetGetallen(r, g1, g2);
                var res = quotMethod.Invoke(r, null);

                Assert.AreEqual(typeof(double), quotMethod.ReturnType, $"Quotient moet een double als returntype hebben , ik kreeg een {res.GetType()} terug");
                Assert.AreEqual(res, g1 / (double)g2, $"Ik testte {g1}/{g2} (als doubles) wat {g1 / (double)g2} moest geven maar ik kreeg {res}");

                g2 = 0;
                SetGetallen(r, g1, g2);
                using (var sw = new StringWriter())
                {
                    Console.SetOut(sw);
                    var res2 = quotMethod.Invoke(r, null);
                    Assert.AreEqual(res2, 0.0, "Ik deed deling door 0 maar kreeg niet 0 als resultaat terug.");
                    Assert.AreEqual(sw.ToString().To.Lower(),Trim(), "kan niet delen door 0", "Deling door 0 moet de juiste foutboodschap tonen, namelijk \"kan niet delen door 0\"");
                }
                
            }
            else Assert.Fail("Quotient methode niet gevonden.");
        }

        private static void SetGetallen(Nummers r, int g1, int g2)
        {
            try
            {
                r.GetType().GetProperty("Getal1").SetValue(r, g1);
                r.GetType().GetProperty("Getal2").SetValue(r, g2);
            }
            catch (Exception)
            {

                Assert.Fail("Zorg dat je eerst de nodige autoprops hebt gemaakt");
            }
            r.GetType().GetProperty("Getal1").SetValue(r, g1);
            r.GetType().GetProperty("Getal2").SetValue(r, g2);
        }

        [TestMethod, Description("Controleert of autoprop Getal1 aanwezig is")]
        public void PropGetal1Test()
        {

            var r = new Nummers();

            bool hasProp = r.GetType().GetProperty("Getal1") != null;
            Assert.AreEqual(true, hasProp, "Geen property Getal1 gevonden");

            var isInt = r.GetType().GetProperty("Getal1").GetMethod.ReturnType;
            Assert.AreEqual(typeof(Int32), isInt, "Property Getal1 niet van int type");

            var isAutoProp = IsAutoProp(r.GetType().GetProperty("Getal1"));
            Assert.AreEqual(isAutoProp, true, "Property Getal1 geen autoprop.");
        }

        [TestMethod, Description("Controleert of autoprop Getal2 aanwezig is")]
        public void PropGetal2Test()
        {

            var r = new Nummers();

            bool hasProp = r.GetType().GetProperty("Getal2") != null;
            Assert.AreEqual(true, hasProp, "Geen property Getal2 gevonden");

            var isInt = r.GetType().GetProperty("Getal2").GetMethod.ReturnType;
            Assert.AreEqual(typeof(Int32), isInt, "Property Getal2 niet van int type");

            var isAutoProp = IsAutoProp(r.GetType().GetProperty("Getal2"));
            Assert.AreEqual(isAutoProp, true, "Property Getal2 geen autoprop.");
        }
        // Bron: https://stackoverflow.com/questions/2210309/how-to-find-out-if-a-property-is-an-auto-implemented-property-with-reflection
        public bool IsAutoProp(PropertyInfo info)
        {
            bool mightBe = info.GetGetMethod()
                               .GetCustomAttributes(
                                   typeof(CompilerGeneratedAttribute),
                                   true
                               )
                               .Any();
            if (!mightBe)
            {
                return false;
            }


            bool maybe = info.DeclaringType
                             .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                             .Where(f => f.Name.Contains(info.Name))
                             .Where(f => f.Name.Contains("BackingField"))
                             .Where(
                                 f => f.GetCustomAttributes(
                                     typeof(CompilerGeneratedAttribute),
                                     true
                                 ).Any()
                             )
                             .Any();

            return maybe;
        }
    }
}
