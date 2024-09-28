//using microsoft.visualstudio.testtools.unittesting;
//using oe.alga.paradigmak;

//namespace oe.alga.tesztek
//{
//    [testclass()]
//    public class feltetelesfeladattarolotesztek
//    {
//        [testmethod()]
//        public void felveszteszt() //f1.(a)
//        {
//            feltetelesfeladattarolo<tesztfeladat> tarolo = new feltetelesfeladattarolo<tesztfeladat>(10);
//            tesztfeladat a = new tesztfeladat("a");
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//        }

//        [testmethod()]
//        [expectedexception(typeof(tarolomegteltkivetel))]
//        public void tulsokatfelveszteszt() //f1.(a)
//        {
//            feltetelesfeladattarolo<tesztfeladat> tarolo = new feltetelesfeladattarolo<tesztfeladat>(5);
//            tesztfeladat a = new tesztfeladat("a");
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//            tarolo.felvesz(a);
//        }

//        [testmethod()]
//        public void mindenvegrehajtasteszt() //f1.(a)
//        {
//            feltetelesfeladattarolo<tesztfeladat> tarolo = new feltetelesfeladattarolo<tesztfeladat>(10);
//            tesztfeladat a = new tesztfeladat("a");
//            tesztfeladat b = new tesztfeladat("b");
//            tarolo.felvesz(a);
//            tarolo.felvesz(b);
//            assert.isfalse(a.vegrehajtott);
//            assert.isfalse(b.vegrehajtott);
//            tarolo.mindentvegrehajt();
//            assert.istrue(a.vegrehajtott);
//            assert.istrue(b.vegrehajtott);
//        }

//        [testmethod()]
//        public void bejaroteszt() //f1.(a)
//        {
//            feltetelesfeladattarolo<tesztfeladat> tarolo = new feltetelesfeladattarolo<tesztfeladat>(10);
//            tesztfeladat a = new tesztfeladat("a");
//            tesztfeladat b = new tesztfeladat("b");
//            tarolo.felvesz(a);
//            tarolo.felvesz(b);
//            string nevek = "";
//            foreach (tesztfeladat u in tarolo)
//            {
//                nevek += u.azonosito;
//            }
//            assert.areequal("ab", nevek);
//        }

//        [testmethod()]
//        public void feltetelesvegrehajtasteszt() //f1.(b)
//        {
//            feltetelesfeladattarolo<tesztfeladat> tarolo = new feltetelesfeladattarolo<tesztfeladat>(10);
//            tesztfeladat a1 = new tesztfeladat("a1");
//            tesztfeladat b1 = new tesztfeladat("b1");
//            tesztfeladat a2 = new tesztfeladat("a2");
//            tarolo.felvesz(a1);
//            tarolo.felvesz(b1);
//            tarolo.felvesz(a2);
//            assert.isfalse(a1.vegrehajtott);
//            assert.isfalse(b1.vegrehajtott);
//            assert.isfalse(a2.vegrehajtott);
//            tarolo.feltetelesvegrehajtas(x => x.azonosito[0] == 'a'); // csak 'a' kezdetűek végrehajtása
//            assert.istrue(a1.vegrehajtott);
//            assert.isfalse(b1.vegrehajtott);
//            assert.istrue(a2.vegrehajtott);
//            tarolo.feltetelesvegrehajtas(x => x.azonosito[0] == 'b'); // csak 'b' kezdetűek végrehajtása
//            assert.istrue(a1.vegrehajtott);
//            assert.istrue(b1.vegrehajtott);
//            assert.istrue(a2.vegrehajtott);
//        }

//        [testmethod()]
//        public void feltetelesfuggosegesvegrehajtasteszt() //f1.(b)
//        {
//            feltetelesfeladattarolo<tesztfuggofeladat> tarolo = new feltetelesfeladattarolo<tesztfuggofeladat>(10);
//            tesztfuggofeladat a1 = new tesztfuggofeladat("a1") { vegrehajthato = true };
//            tesztfuggofeladat b1 = new tesztfuggofeladat("b1") { vegrehajthato = true };
//            tesztfuggofeladat a2 = new tesztfuggofeladat("a2") { vegrehajthato = false };
//            tarolo.felvesz(a1);
//            tarolo.felvesz(b1);
//            tarolo.felvesz(a2);
//            assert.isfalse(a1.vegrehajtott);
//            assert.isfalse(b1.vegrehajtott);
//            assert.isfalse(a2.vegrehajtott);
//            tarolo.feltetelesvegrehajtas(x => x.azonosito[0] == 'a' && x.fuggosegteljesul); // csak 'a' kezdetű és végrehajtható
//            assert.istrue(a1.vegrehajtott);
//            assert.isfalse(b1.vegrehajtott);
//            assert.isfalse(a2.vegrehajtott);
//            tarolo.feltetelesvegrehajtas(x => x.azonosito[0] == 'b' && x.fuggosegteljesul); // csak 'b' kezdetű és végrehajtható
//            assert.istrue(a1.vegrehajtott);
//            assert.istrue(b1.vegrehajtott);
//            assert.isfalse(a2.vegrehajtott);
//            a2.vegrehajthato = true;
//            tarolo.feltetelesvegrehajtas(x => x.azonosito[0] == 'a' && x.fuggosegteljesul); // csak 'a' kezdetű és végrehajtható
//            assert.istrue(a1.vegrehajtott);
//            assert.istrue(b1.vegrehajtott);
//            assert.istrue(a2.vegrehajtott);
//        }

//        [testmethod()]
//        public void feltetelesbejaroteszt() //f3.(b)
//        {
//            feltetelesfeladattarolo<tesztfuggofeladat> tarolo = new feltetelesfeladattarolo<tesztfuggofeladat>(10);
//            tarolo.bejarofeltetel = (x => x.fuggosegteljesul);
//            tesztfuggofeladat a = new tesztfuggofeladat("a") { vegrehajthato = true };
//            tesztfuggofeladat b = new tesztfuggofeladat("b") { vegrehajthato = false };
//            tesztfuggofeladat c = new tesztfuggofeladat("c") { vegrehajthato = true };
//            tarolo.felvesz(a);
//            tarolo.felvesz(b);
//            tarolo.felvesz(c);
//            string nevek = "";
//            foreach (tesztfeladat u in tarolo)
//            {
//                nevek += u.azonosito;
//            }
//            assert.areequal("ac", nevek);
//        }
//    }
//}
