using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Chatterbox.Gui.Utils
{

    public sealed class Emotocon
    {

        public static List<Emotocon> Emotocons { get; private set; }

        static Emotocon()
        {
            Emotocons = new List<Emotocon> {
                                                  new Emotocon ( "allthethings.png", "allthethings" ),
                                                  new Emotocon ( "android.png", "android" ),
                                                  new Emotocon ( "angry.png", "angry" ),
                                                  new Emotocon ( "areyoukiddingme.png", "areyoukiddingme" ),
                                                  new Emotocon ( "ashton.png", "ashton" ),
                                                  new Emotocon ( "awthanks.png", "awthanks" ),
                                                  new Emotocon ( "awyea.png", "awyea" ),
                                                  new Emotocon ( "badass.png", "badass" ),
                                                  new Emotocon ( "badpokerface.png", "badpokerface" ),
                                                  new Emotocon ( "basket.png", "basket" ),
                                                  new Emotocon ( "beer.png", "beer" ),
                                                  new Emotocon ( "bigsmile.png", "bigsmile" ),
                                                  new Emotocon ( "boom.gif", "boom" ),
                                                  new Emotocon ( "bumble.png", "bumble" ),
                                                  new Emotocon ( "bunny.png", "bunny" ),
                                                  new Emotocon ( "cadbury.png", "cadbury" ),
                                                  new Emotocon ( "cake.png", "cake" ),
                                                  new Emotocon ( "candycorn.png", "candycorn" ),
                                                  new Emotocon ( "caruso.png", "caruso" ),
                                                  new Emotocon ( "ceilingcat.png", "ceilingcat" ),
                                                  new Emotocon ( "cerealguy.png", "cerealguy" ),
                                                  new Emotocon ( "cerealspit.png", "cerealspit" ),
                                                  new Emotocon ( "challengeaccepted.png", "challengeaccepted" ),
                                                  new Emotocon ( "chewy.png", "chewy" ),
                                                  new Emotocon ( "chocobunny.png", "chocobunny" ),
                                                  new Emotocon ( "chompy.gif", "chompy" ),
                                                  new Emotocon ( "chris.png", "chris" ),
                                                  new Emotocon ( "chucknorris.png", "chucknorris" ),
                                                  new Emotocon ( "coffee.png", "coffee" ),
                                                  new Emotocon ( "content.png", "content" ),
                                                  new Emotocon ( "cool.png", "cool" ),
                                                  new Emotocon ( "cornelius.png", "cornelius" ),
                                                  new Emotocon ( "cry.png", "cry" ),
                                                  new Emotocon ( "dance.gif", "dance" ),
                                                  new Emotocon ( "dealwithit.gif", "dealwithit" ),
                                                  new Emotocon ( "derp.png", "derp" ),
                                                  new Emotocon ( "disapproval.png", "disapproval" ),
                                                  new Emotocon ( "dosequis.png", "dosequis" ),
                                                  new Emotocon ( "drevil.png", "drevil" ),
                                                  new Emotocon ( "ducreux.png", "ducreux" ),
                                                  new Emotocon ( "dumb.png", "dumb" ),
                                                  new Emotocon ( "dumbbitch.png", "dumbbitch" ),
                                                  new Emotocon ( "embarrassed.png", "embarrassed" ),
                                                  new Emotocon ( "facepalm.png", "facepalm" ),
                                                  new Emotocon ( "fap.png", "fap" ),
                                                  new Emotocon ( "farnsworth.png", "farnsworth" ),
                                                  new Emotocon ( "firstworldproblem.png", "firstworldproblem" ),
                                                  new Emotocon ( "footinmouth.png", "footinmouth" ),
                                                  new Emotocon ( "foreveralone.png", "foreveralone" ),
                                                  new Emotocon ( "freddie.png", "freddie" ),
                                                  new Emotocon ( "frown.png", "frown" ),
                                                  new Emotocon ( "fry.png", "fry" ),
                                                  new Emotocon ( "fuckyeah.png", "fuckyeah" ),
                                                  new Emotocon ( "fwp.png", "fwp" ),
                                                  new Emotocon ( "gangnamstyle.gif", "gangnamstyle" ),
                                                  new Emotocon ( "garret.png", "garret" ),
                                                  new Emotocon ( "gasp.png", "gasp" ),
                                                  new Emotocon ( "gates.png", "gates" ),
                                                  new Emotocon ( "gaytroll.gif", "gaytroll" ),
                                                  new Emotocon ( "ghost.png", "ghost" ),
                                                  new Emotocon ( "greenbeer.png", "greenbeer" ),
                                                  new Emotocon ( "gtfo.png", "gtfo" ),
                                                  new Emotocon ( "happytear.gif", "happytear" ),
                                                  new Emotocon ( "haveaseat.png", "haveaseat" ),
                                                  new Emotocon ( "heart.png", "heart" ),
                                                  new Emotocon ( "hipster.png", "hipster" ),
                                                  new Emotocon ( "ilied.png", "ilied" ),
                                                  new Emotocon ( "indeed.png", "indeed" ),
                                                  new Emotocon ( "innocent.png", "innocent" ),
                                                  new Emotocon ( "iseewhatyoudidthere.png", "iseewhatyoudidthere" ),
                                                  new Emotocon ( "itsatrap.png", "itsatrap" ),
                                                  new Emotocon ( "jackie.png", "jackie" ),
                                                  new Emotocon ( "jobs.png", "jobs" ),
                                                  new Emotocon ( "kennypowers.png", "kennypowers" ),
                                                  new Emotocon ( "kiss.png", "kiss" ),
                                                  new Emotocon ( "krang.gif", "krang" ),
                                                  new Emotocon ( "kwanzaa.png", "kwanzaa" ),
                                                  new Emotocon ( "lincoln.png", "lincoln" ),
                                                  new Emotocon ( "lol.png", "lol" ),
                                                  new Emotocon ( "lolwut.png", "lolwut" ),
                                                  new Emotocon ( "megusta.png", "megusta" ),
                                                  new Emotocon ( "menorah.png", "menorah" ),
                                                  new Emotocon ( "mike.png", "mike" ),
                                                  new Emotocon ( "moneymouth.png", "moneymouth" ),
                                                  new Emotocon ( "ninja.png", "ninja" ),
                                                  new Emotocon ( "notbad.png", "notbad" ),
                                                  new Emotocon ( "nothingtodohere.png", "nothingtodohere" ),
                                                  new Emotocon ( "notsureif.png", "notsureif" ),
                                                  new Emotocon ( "notsureifgusta.png", "notsureifgusta" ),
                                                  new Emotocon ( "obama.png", "obama" ),
                                                  new Emotocon ( "ohcrap.png", "ohcrap" ),
                                                  new Emotocon ( "ohgodwhy.jpeg", "ohgodwhy." ),
                                                  new Emotocon ( "okay.png", "okay" ),
                                                  new Emotocon ( "omg.png", "omg" ),
                                                  new Emotocon ( "oops.png", "oops" ),
                                                  new Emotocon ( "orly.png", "orly" ),
                                                  new Emotocon ( "pbr.png", "pbr" ),
                                                  new Emotocon ( "pete.png", "pete" ),
                                                  new Emotocon ( "philosoraptor.png", "philosoraptor" ),
                                                  new Emotocon ( "pingpong.png", "pingpong" ),
                                                  new Emotocon ( "pirate.gif", "pirate" ),
                                                  new Emotocon ( "pokerface.png", "pokerface" ),
                                                  new Emotocon ( "poo.png", "poo" ),
                                                  new Emotocon ( "present.png", "present" ),
                                                  new Emotocon ( "pumpkin.png", "pumpkin" ),
                                                  new Emotocon ( "rageguy.png", "rageguy" ),
                                                  new Emotocon ( "rebeccablack.png", "rebeccablack" ),
                                                  new Emotocon ( "reddit.png", "reddit" ),
                                                  new Emotocon ( "romney.png", "romney" ),
                                                  new Emotocon ( "rudolph.png", "rudolph" ),
                                                  new Emotocon ( "sadpanda.png", "sadpanda" ),
                                                  new Emotocon ( "sadtroll.png", "sadtroll" ),
                                                  new Emotocon ( "samuel.png", "samuel" ),
                                                  new Emotocon ( "santa.png", "santa" ),
                                                  new Emotocon ( "scumbag.png", "scumbag" ),
                                                  new Emotocon ( "sealed.png", "sealed" ),
                                                  new Emotocon ( "seomoz.png", "seomoz" ),
                                                  new Emotocon ( "shamrock.png", "shamrock" ),
                                                  new Emotocon ( "shrug.png", "shrug" ),
                                                  new Emotocon ( "skyrim.png", "skyrim" ),
                                                  new Emotocon ( "slant.png", "slant" ),
                                                  new Emotocon ( "smile.png", "smile" ),
                                                  new Emotocon ( "stare.png", "stare" ),
                                                  new Emotocon ( "straightface.png", "straightface" ),
                                                  new Emotocon ( "success.png", "success" ),
                                                  new Emotocon ( "sweetjesus.png", "sweetjesus" ),
                                                  new Emotocon ( "tableflip.png", "tableflip" ),
                                                  new Emotocon ( "taft.png", "taft" ),
                                                  new Emotocon ( "tea.png", "tea" ),
                                                  new Emotocon ( "thumbs_down.png", "thumbs_down" ),
                                                  new Emotocon ( "thumbs_up.png", "thumbs_up" ),
                                                  new Emotocon ( "tongue.png", "tongue" ),
                                                  new Emotocon ( "tree.png", "tree" ),
                                                  new Emotocon ( "troll.png", "troll" ),
                                                  new Emotocon ( "truestory.png", "truestory" ),
                                                  new Emotocon ( "trump.png", "trump" ),
                                                  new Emotocon ( "turkey.png", "turkey" ),
                                                  new Emotocon ( "twss.png", "twss" ),
                                                  new Emotocon ( "washington.png", "washington" ),
                                                  new Emotocon ( "wat.png", "wat" ),
                                                  new Emotocon ( "wink.png", "wink" ),
                                                  new Emotocon ( "winktongue.gif", "winktongue" ),
                                                  new Emotocon ( "wtf.png", "wtf" ),
                                                  new Emotocon ( "yey.png", "yey" ),
                                                  new Emotocon ( "yodawg.png", "yodawg" ),
                                                  new Emotocon ( "yuno.png", "yuno" ),
                                                  new Emotocon ( "zoidberg.png", "zoidberg" ),
                                                  new Emotocon ( "zzz.gif", "zzz" ),
                                                  


                                              };
#if !RELEASE
            checkForConflicts();
#endif

        }

        private static void checkForConflicts()
        {
            for (int i = 0; i < Emotocons.Count; )
            {
                Emotocon e = Emotocons[i++];
                for (int other = i; other < Emotocons.Count; )
                {
                    Emotocon e2 = Emotocons[other++];
                    foreach (string s2 in e2.Atlases)
                    {
                        foreach (string s in e.Atlases.Where(s => s.Equals(s2, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            MessageBox.Show("Conflict Found { " + s + ", " + s2 + " }");
                            throw new Exception("Conflict found");
                        }
                    }
                }
            }
        }

        public static string GetImage(string byAtlas)
        {
            foreach (Emotocon e in Emotocons.Where(e => e.Atlases.Any(t => t.Equals(byAtlas, StringComparison.CurrentCultureIgnoreCase))))
            {
                return e.Url;
            }

            return string.Empty;
        }

        public string Url { get; set; }
        public string[] Atlases { get; set; }

        public Emotocon(string url, params string[] atlases)
        {
            Url = url;
            Atlases = atlases;
        }




    }

}
