using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Kinkkuhefe : PhysicsGame
{
	// TAUSTAKUVAT
	Image valikkoTausta = LoadImage("aloitusValikko"); 		// Ladataan keittiöstä kuva alkuvalikon taustaksi.
	Image pelinTausta = LoadImage("pelinTausta"); 			// Ladataan todellisen toiminnan aikainen näkymä.


	// PISTEET KERTYY TÄHÄN
	int pisteenLasku = 0; 


	// PARI LISTAA
	List<PhysicsObject> ainekset = new List<PhysicsObject>();	// Thö kinkun & aineksien lista
	List<String> lisattyKinkkuunString = new List<String> (); 	// Kinkkuun lisättyjen tuotteiden lista


	// OBJEKTIT
	PhysicsObject kinkku;			// 1.
	PhysicsObject elamansuola;		// 2.
	PhysicsObject hksininen; 		// 3.
	PhysicsObject jackdaniels;		// 4.
	PhysicsObject kebabkastike;		// 5.
	PhysicsObject lanttu;			// 6.
	PhysicsObject kossu;			// 7.
	PhysicsObject mandariini;		// 8.
	PhysicsObject marsipaani;		// 9.
	PhysicsObject rakuuna;			// 10.
	PhysicsObject msmjauhe;			// 11. 
	PhysicsObject mustaherukka;		// 12.
	PhysicsObject mustakitaturska;	// 13.
	PhysicsObject mustapippuri;		// 14.
	PhysicsObject sukkahousut;		// 15.
	PhysicsObject tilli;			// 16.

	PhysicsObject logo;				// Logo fysiikkaolioksi

	/*
	// MITAT AINEKSIEN LAITTOON
	Image ruiskumitta = LoadImage("ruiskumitta"); 				// Lisätään ruiskumitta
	Image loylykauha = LoadImage("loylykauha"); 				// Lisätään löylykauha
	Image mitta1dl = LoadImage("mitta1dl"); 					// Lisätään desimitan
	Image mittaruokalusikka = LoadImage("mittaruokalusikka");	// Lisätään ruokalusikka
	Image mittateelusikka = LoadImage("mittateelusikka"); 		// Lisätään teelusikka 
	Image soppakauha = LoadImage("soppakauha"); 				// Lisätään soppakauha
	*/

	// LISÄÄ SOKERISIA JUTKUTUSHÄRPÄKKEITÄ
	/*
	Image niittivyo = LoadImage("niittivyo"); 					// Lisätään niittivyö objekti 
	Image pelargonia = LoadImage("pelargonia"); 				// Lisätään pelargonian objekti 
	Image radio = LoadImage("radio"); 							// Lisätään radion objekti 
	Image xboxohjain = LoadImage("xboxohjain"); 				// Lisätään xboxohjain
	Image saunavihta = LoadImage("saunavihta"); 				// Lisätään saunavihta
	*/


	// LUODAAN ESC:illä AVATTAVA VALIKKO 
	void Valikko(){
		ClearAll(); 											// Tyhjennetään kenttä kaikista objekteista
		MultiSelectWindow valikko = new MultiSelectWindow("", "JOO JOO KINKKUU TULILLE", "HALL OF KINKKUHEFE", "SYÖN MIELUUMMIN ANANASPIZZAA...");
		Level.Background.Image = valikkoTausta; 				// Ladataan keittiöstä kuva valikon taustaksi
		Add(valikko);


		//HANDLERIT
		valikko.DefaultCancel = 3;								// Kolmas nappi poistuu pelistä, kuten ESC -> YES
		valikko.AddItemHandler(2, Exit);						// Poistu pelistä, kun nappia kaksi painetaan
		valikko.AddItemHandler(0, PeliKayntiin);				// Aloittaa pelin kun ylintä nappia painetaan
		valikko.AddItemHandler(1, HallOfKinkkuhefe);			// Näytetään top-score listaus 

		logo = PhysicsObject.CreateStaticObject(923, 538);		// Logo alkuvalikon taustalle
		logo.Image = LoadImage("hefeLogo");						// Logokuva
		logo.Position = new Vector (0, -50);					// Logokuvan sijainti ruudulla
		Add (logo, 0);											// Lisätään KH logo 0:een kerrokseen
	}


	// PÄÄOHJELMA
	public override void Begin ()
	{
		// YLEISET IKKUNAN ASETUKSET
		SetWindowSize(1920, 1080);								// Ikkunan koko sama kuin taustakuvien resoluutio
		IsFullScreen = true; 									// Peli asetetaan kokonäytölle.	
		Camera.ZoomToLevel(100);								// Koko tausta näkyvillä.
		Mouse.IsCursorVisible = true; 							// Hiiri näkyviin.
		SmoothTextures = false;									// Reunojen pehmennys pois käytöstä.
		Level.Background.Image = pelinTausta; 					// Ladataan keittiöstä kuva pelin taustaksi.
		Valikko();												// Kutsutaan valikkoa heti alkuun, niin ei tarvitse pelaajan ESCiä painella.
	}


	// ITSE PELIIN
	void PeliKayntiin()
	{
		Remove (logo);												// Tyhjennetään kenttä kaikista objekteista, mutta toki vain kerran
		Level.Background.Image = pelinTausta; 					// Ladataan keittiöstä kuva pelin taustaksi
		Ainekset(ainekset);										// Lisätään ainekset kentälle, kun on valittu, että lähdetään paistamaan kinkkua.
		// HIIREN KÄYTTÖ OBJEKTIEN LIIKUTTELUUN & TUTKIMISEEN
		Mouse.Listen (MouseButton.Left, ButtonState.Pressed, KuunteleLiiketta2, "Jos ei koordinaatio riitä ;D");
		Mouse.Listen (MouseButton.Left, ButtonState.Down, KuunteleLiiketta, "Lisää aineksia kinkkuun mausteeksi.");
		Mouse.Listen (MouseButton.Left, ButtonState.Released, OnkoKinkunPaalla, null);
		// VALIKKOON MENEMINEN
		Keyboard.Listen (Key.Escape, ButtonState.Pressed, Valikko, "Avaa valikko");
	}


	// HIGHSCORE TAULUKKO
	void HallOfKinkkuhefe()
	{
		// Fetchaa suoraa koneen käyttäjän nimi joka on oletuksena topscore nicki
		// Kun tulee uusi highscore niin JOHN CENAA / tietyn pistemäärän yli
		// Vois heittää jonku hall of fame taustan


		// VALIKKOON MENEMINEN
		Keyboard.Listen (Key.Escape, ButtonState.Pressed, Valikko, "Avaa valikko");
	}


	// AINEKSIEN LISÄÄMINEN KINKKUUN
	void OnkoKinkunPaalla(){
		MessageDisplay.Clear ();																// Tyhjennetään tekstiruutu edellisestä viisastelusta.

		if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (elamansuola)) {						// Suolan lisäys kinkkuun
			MultiSelectWindow suolaValikko = new MultiSelectWindow ("Kuinka suolaista meinasit?", "Ripaus sinne tänne", "Kourallinen", "Kilpirauhasen räjäytys"); 
			elamansuola.Destroy ();
			Add (suolaValikko);
			lisattyKinkkuunString.Add ("suolaa");
			//suolaValikko.ItemSelected += KommentitAineksista;
			int i = suolaValikko.SelectedIndex;
			AinestenMaara (suolaValikko.SelectedIndex);
		}

		else if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (jackdaniels)) {				// Jack Danielssin lisäys kinkkuun
			MultiSelectWindow jackdanielsValikko = new MultiSelectWindow ("Kinkku uimaan viskiin?", "No ei, ihan ujosti päälle", "Puolet meni jo kokkiin", "Järvisuomi"); 
			jackdaniels.Destroy ();
			lisattyKinkkuunString.Add ("Jack Daniels viskiä");
			//jackdanielsValikko.ItemSelected += KommentitAineksista;
			Add (jackdanielsValikko);
			int i = jackdanielsValikko.SelectedIndex;
			AinestenMaara (jackdanielsValikko.SelectedIndex);
		}
		else if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (hksininen)) {					// Makkaran lisäys kinkkuun
			MultiSelectWindow hksininenValikko = new MultiSelectWindow ("Ootsää mies vai hanhi?", "Yks kyrsä ny alkuun", "Metri-Heikki", "Norsunsuoli"); 
			hksininen.Destroy ();
			lisattyKinkkuunString.Add ("makkaraa");
			//hksininenValikko.ItemSelected += KommentitAineksista;
			Add (hksininenValikko);
			int i = hksininenValikko.SelectedIndex;
			AinestenMaara (hksininenValikko.SelectedIndex);
		}
	}


	// LISÄTTYJEN AINESTEN MÄÄRÄN RAJOITTAMINEN
	void AinestenMaara(int indeksi){
		int i = indeksi;
		if (i == 0) {
			pisteenLasku = pisteenLasku + 1;
		} 
		else if (i == 1) {
			pisteenLasku += 3;
		}
	}

	/*
// KOMMENTTI AINEKSISTA KUN NE LISÄTÄÄN
	void KommentitAineksista(int i){

		switch (i){
		case 0 :
			MessageDisplay.Add ("Nössösti lisätty!");
			MessageDisplay.MaxMessageCount = 0;
			break;
		case 1:
			MessageDisplay.Add ("Voi veljet q:-D");
			MessageDisplay.MaxMessageCount = 0;
			break;
		case 2:
			MessageDisplay.Add ("Päätyyn asti!");
			MessageDisplay.MaxMessageCount = 0;
			break;
		}
	}	*/


	// HIIREN KUUNTELU ELI MITÄ TAPAHTUU KUN VASEMMALLA HIIRELLÄ KLIKATAAN OHI
	void KuunteleLiiketta2()
	{
		MessageDisplay.Add ("TARTU KUIN MIES!");
		MessageDisplay.MaxMessageCount = 0;
	}


	// HIIREN KUUNTELU ELI MITÄ TAPAHTUU KUN VASEN HIIRI ON PAINETTU POHJAAN
	void KuunteleLiiketta()
	{   
		MessageDisplay.Clear();							// Tyhjennetään tekstiruutu edellisestä viisastelusta.
		/*
	if (lisattyKinkkuunString.Count >= 2) {
		Widget ruutu1 = new Widget (100.0, 50.0);
		Label lisatytmausteet = new Label ("Maustamisen puolesta aika laittaa kinkku uuniin.");
		ruutu1.Add (lisatytmausteet);
		Add (ruutu1);
	}
	*/
		if (Mouse.IsCursorOn (elamansuola)) {
			elamansuola.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Käytä ensi kerralla Himalajan suolaa");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (hksininen)) {
			hksininen.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Vähä kyrsää... Perus suomalaista!");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (jackdaniels)) {
			jackdaniels.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Sullahan on ihan kehittynyt maku.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (kebabkastike)) {
			kebabkastike.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Oi että tää ei koskaan jätä kylmäks.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (lanttu)) {
			lanttu.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Meidän kunnioitettu puheenjohtaja.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (kossu)) {
			kossu.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("RAI RAI!");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (mandariini)) {
			mandariini.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Ootsää mies vai hanhi?");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (marsipaani)) {
			marsipaani.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Herkkuperse!");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (rakuuna)) {
			rakuuna.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Ei susta kyllä kokkia tule.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (msmjauhe)) {
			msmjauhe.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Et taida tietää mitä tää on...");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (mustaherukka)) {
			mustaherukka.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Rohkee veto, sekaan vaan!");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (mustakitaturska)) {
			mustakitaturska.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Kalaa maailman pimeimmistä vesistä.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (mustapippuri)) {
			mustapippuri.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Turvallisin vaihtoehto... Vässykkä.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (sukkahousut)) {
			sukkahousut.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Kannattaa kääriä kinkku tähän ettei kuivu.");
			MessageDisplay.MaxMessageCount = 0;
		} 
		else if (Mouse.IsCursorOn (tilli)) {
			tilli.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("Vaimo taas laittanu tillilihaa...");
			MessageDisplay.MaxMessageCount = 0;
		}
	}


	// LUODAAN OBJEKTEISTA LISTA & LISÄTÄÄN KAIKKI OBJEKTIT PELIIN
	void Ainekset(List<PhysicsObject> ainekset)
	{
		// THÖ KINKKU
		kinkku = PhysicsObject.CreateStaticObject(Level.Width * 0.3, Level.Height * 0.2);
		kinkku.Image = LoadImage("kinkku");							// 1. Lisätään kinkku
		kinkku.Position = new Vector (-270, 0);
		Add (kinkku, 0);
		elamansuola = new PhysicsObject (Level.Width * 0.05, Level.Height * 0.1);
		elamansuola.Image = LoadImage("elamansuola"); 				// 2. Lisätään suolapurkki
		elamansuola.Position = new Vector (200, 50);
		elamansuola.Tag = "elamansuola";
		ainekset.Add (elamansuola);
		Add (elamansuola, 1);										
		hksininen = new PhysicsObject (Level.Width * 0.15, Level.Height * 0.075);
		hksininen.Image = LoadImage("hksininen"); 					// 3. Lisätään HK:n sininen eli makkara
		hksininen.Position = new Vector (400, 50);
		hksininen.Tag = "hksininen";
		ainekset.Add (hksininen);
		Add (hksininen, 1);
		jackdaniels = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.25);
		jackdaniels.Image = LoadImage("jackdaniels"); 				// 4. Lisätään Jack Daniels viskipullo
		jackdaniels.X = 200;
		jackdaniels.Y = -100;
		jackdaniels.Tag = "jackdaniels";
		Add (jackdaniels, 1);
		kebabkastike = new PhysicsObject (Level.Width * 0.15, Level.Height * 0.15);
		kebabkastike.Image = LoadImage("kebabkastike"); 			// 5. Lisätään kebabkastikepurkit 
		kebabkastike.X = 300;
		kebabkastike.Y = -100;
		kebabkastike.Tag = "kebabkastike";
		Add (kebabkastike, 1);
		lanttu = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.1);
		lanttu.Image = LoadImage("lanttu"); 						// 6. Lisätään kolmen lanttua
		lanttu.X = 400;
		lanttu.Y = -100;
		lanttu.Tag = "lanttu";
		Add (lanttu, 1);
		kossu = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.25);
		kossu.Image = LoadImage("kossu"); 							// 7. Lisätään Koskenkorva viinapullo
		kossu.X = 500;
		kossu.Y = 100;
		kossu.Tag = "kossu";
		Add (kossu, 1);
		mandariini = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.1);
		mandariini.Image = LoadImage("mandariini"); 				// 8. Lisätään mandariini
		mandariini.X = 600;
		mandariini.Y = -100;
		mandariini.Tag = "mandariini";
		Add (mandariini, 1);
		marsipaani = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.15);
		marsipaani.Image = LoadImage("marsipaani"); 				// 9. Lisätään marsipaani
		marsipaani.X = 700;
		marsipaani.Y = 100;
		marsipaani.Tag = "marsipaani";
		Add (marsipaani, 1);
		rakuuna = new PhysicsObject (Level.Width * 0.025, Level.Height * 0.07);
		rakuuna.Image = LoadImage("rakuuna");						// 10. Lisätään rakuuna maustepurkki
		rakuuna.X = -100;
		rakuuna.Y = -100;
		rakuuna.Tag = "rakuuna";
		Add (rakuuna, 1);
		msmjauhe = new PhysicsObject (Level.Width * 0.05, Level.Height * 0.05);
		msmjauhe.Image = LoadImage("msmjauhe"); 					// 11. Lisätään MSM -jauhe kippo 
		msmjauhe.X = -200;
		msmjauhe.Y = 100;
		msmjauhe.Tag = "msmjauhe";
		Add (msmjauhe, 1);
		mustaherukka = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.1);
		mustaherukka.Image = LoadImage("mustaherukka"); 			// 12. Lisätään mustaherukat 
		mustaherukka.X = -300;
		mustaherukka.Y = -100;
		mustaherukka.Tag = "mustaherukka";
		Add (mustaherukka, 1);
		mustakitaturska = new PhysicsObject (Level.Width * 0.25, Level.Height * 0.1);
		mustakitaturska.Image = LoadImage("mustakitaturska"); 		// 13. Lisätään mustakitaturskan
		mustakitaturska.X = -400;
		mustakitaturska.Y = 100;
		mustakitaturska.Tag = "mustakitaturska";
		Add (mustakitaturska, 1);
		mustapippuri = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.1);
		mustapippuri.Image = LoadImage("mustapippuri"); 			// 14. Lisätään mustapippurit
		mustapippuri.X = -500;
		mustapippuri.Y = -100;
		mustapippuri.Tag = "mustapippuri";
		Add (mustapippuri, 1);
		sukkahousut = new PhysicsObject (Level.Width * 0.15, Level.Height * 0.25);
		sukkahousut.Image = LoadImage("sukkahousut"); 				// 15. Lisätään sukkahousut
		sukkahousut.X = -600;
		sukkahousut.Y = 100;
		sukkahousut.Tag = "sukkahousut";
		Add (sukkahousut, 1);
		tilli = new PhysicsObject (Level.Width * 0.125, Level.Height * 0.125);
		tilli.Image = LoadImage("tilli"); 							// 16. Lisätään tilli
		tilli.X = -600;
		tilli.Y = -100;
		tilli.Tag = "tilli";
		Add (tilli, 1);
	}


}