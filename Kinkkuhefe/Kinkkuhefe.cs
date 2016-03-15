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

	// HOK = Hall Of Kinkkuhefe
	ScoreList HOK = new ScoreList(5, false, 0);

	// KINKKUUN LISATTYJEN AINESTEN MÄÄRÄ
	int ainestenMaara = 0;

	// PELAAJAN PISTEET
	int pisteet = 0;

	// MUSAT EI VAAB TOIMI ;G
	// SoundEffect taustamusa = LoadSoundEffect("JCteema");
	SoundEffect radiosta = LoadSoundEffect("JC");

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

	PhysicsObject radio;			// Lisätään radio ihan taustalle.
	PhysicsObject logo;				// Logo fysiikkaolioksi



	// LUODAAN ESC:illä AVATTAVA VALIKKO 
	void Valikko()
	{
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
		SetWindowSize(1920, 1080);									// Ikkunan koko sama kuin taustakuvien resoluutio
		IsFullScreen = true; 										// Peli asetetaan kokonäytölle.	
		Camera.ZoomToLevel(100);									// Koko tausta näkyvillä.
		Mouse.IsCursorVisible = true; 								// Hiiri näkyviin.
		SmoothTextures = false;										// Reunojen pehmennys pois käytöstä.
		Level.Background.Image = pelinTausta; 						// Ladataan keittiöstä kuva pelin taustaksi.
		HOK = DataStorage.TryLoad<ScoreList>(HOK, "pojot.xml" );	// Hall of fame listaukselle data
		Valikko();													// Kutsutaan valikkoa heti alkuun, niin ei tarvitse pelaajan ESCiä painella
	}
		
	// ITSE PELIIN
	void PeliKayntiin()
	{
		Remove (logo);											// Tyhjennetään kentältä logo
		Level.Background.Image = pelinTausta; 					// Ladataan keittiöstä kuva pelin taustaksi
		Ainekset(ainekset);										// Lisätään ainekset kentälle, kun on valittu, että lähdetään paistamaan kinkkua.


		// HIIREN KÄYTTÖ OBJEKTIEN LIIKUTTELUUN & TUTKIMISEEN
		Mouse.Listen (MouseButton.Left, ButtonState.Pressed, KuunteleLiiketta2, "Jos ei koordinaatio riitä ;D");
		Mouse.Listen (MouseButton.Left, ButtonState.Down, KuunteleLiiketta, "Lisää aineksia kinkkuun mausteeksi.");
		Mouse.Listen (MouseButton.Left, ButtonState.Released, OnkoKinkunPaalla, null);


		// VALIKKOON MENEMINEN
		Keyboard.Listen (Key.Escape, ButtonState.Pressed, Valikko, "Avaa valikko");
	}
		
	// PISTEIDEN TALLENNUS HALL OF KINKKUHEFEÄ VARTEN
	void TallennaPisteet( Window sender )
	{
		DataStorage.Save<ScoreList>(HOK, "pojot.xml");
	}
		
	// HIGHSCORE TAULUKKO
	void HallOfKinkkuhefe()
	{
		HighScoreWindow hallOfKinkku = new HighScoreWindow("                                 HALL OF KINKKUHEFE", 
			"Pääsit kinkunpaiston all-staareihin pistein %p! Anna nickisi:", HOK, pisteet);
		hallOfKinkku.Closed += TallennaPisteet;
		Add(hallOfKinkku);
		hallOfKinkku.Closed += delegate (Window sender) 
		{
			Valikko();
		};

		// Fetchaa suoraa koneen käyttäjän nimi joka on oletuksena topscore nicki
		// Kun tulee uusi highscore niin JOHN CENAA / tietyn pistemäärän yli


		// VALIKKOON MENEMINEN
		Keyboard.Listen (Key.Escape, ButtonState.Pressed, Valikko, "Avaa valikko");
	}

	// AINEKSIEN LISÄÄMINEN KINKKUUN
	void OnkoKinkunPaalla()
	{
		MessageDisplay.Clear ();																// Tyhjennetään tekstiruutu edellisestä viisastelusta.

		if (lisattyKinkkuunString.Count > 2) {
			Widget ruutu1 = new Widget (300, 50.0);
			Label lisatytmausteet = new Label ("Mausteiden puolesta aika laittaa kinkku uuniin.");
			ruutu1.Add (lisatytmausteet);
			Add (ruutu1);
		}
		else if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (elamansuola)) {						// Suolan lisäys kinkkuun
			MultiSelectWindow suolaValikko = new MultiSelectWindow ("Kuinka suolaista meinasit?", "Ripaus sinne tänne", "Kourallinen", "Kilpirauhasen räjäytys"); 
			elamansuola.Destroy ();
			Add (suolaValikko);
			lisattyKinkkuunString.Add ("suolaa");
			suolaValikko.ItemSelected += KommentitAineksista;
			int i = suolaValikko.SelectedIndex;
			AinestenMaara (suolaValikko.SelectedIndex);
		}
		else if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (jackdaniels)) {					// Jack Danielssin lisäys kinkkuun
			MultiSelectWindow jackdanielsValikko = new MultiSelectWindow ("Kinkku uimaan viskiin?", "No ei, ihan ujosti päälle", "Puolet meni jo kokkiin", "Järvisuomi"); 
			jackdaniels.Destroy ();
			lisattyKinkkuunString.Add ("Jack Daniels viskiä");
			jackdanielsValikko.ItemSelected += KommentitAineksista;
			Add (jackdanielsValikko);
			int i = jackdanielsValikko.SelectedIndex;
			AinestenMaara (jackdanielsValikko.SelectedIndex);
		}
		else if (Mouse.IsCursorOn (kinkku) && Mouse.IsCursorOn (hksininen)) {					// Makkaran lisäys kinkkuun
			MultiSelectWindow hksininenValikko = new MultiSelectWindow ("Ootsää mies vai hanhi?", "Yks kyrsä ny alkuun", "Metri-Heikki", "Norsunsuoli"); 
			hksininen.Destroy ();
			lisattyKinkkuunString.Add ("makkaraa");
			hksininenValikko.ItemSelected += KommentitAineksista;
			Add (hksininenValikko);
			int i = hksininenValikko.SelectedIndex;
			AinestenMaara (hksininenValikko.SelectedIndex);
		}
	}

	// LISÄTTYJEN AINESTEN MÄÄRÄN RAJOITTAMINEN
	void AinestenMaara(int indeksi)
	{
		int i = indeksi;
		if (i == 0) {
			ainestenMaara = ainestenMaara + 1;
		} 
		else if (i == 1) {
			ainestenMaara += 3;
		}
	}

	// KOMMENTTI AINEKSISTA KUN NE LISÄTÄÄN
	void KommentitAineksista(int i)
	{
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
			MessageDisplay.Add ("Hautaan asti!");
			MessageDisplay.MaxMessageCount = 0;
			break;
		}
	}

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
			MessageDisplay.Add ("Olen erittäin pahanmakuinen.");
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
		/*
		else if (Mouse.IsCursorOn (radio)) {
			radio.Position = Mouse.PositionOnWorld;
			MessageDisplay.Add ("JOHN CENAAA");
			MessageDisplay.MaxMessageCount = 0;
			radiosta.Play ();
		}
		*/
	}
		
	// LUODAAN OBJEKTEISTA LISTA & LISÄTÄÄN KAIKKI OBJEKTIT PELIIN
	void Ainekset(List<PhysicsObject> ainekset)
	{
		// THÖ RADIO
		radio = PhysicsObject.CreateStaticObject(Level.Width * 0.3, Level.Height * 0.2);
		radio.Image = LoadImage("radio");							// Lisätään radio taustalle
		radio.Position = new Vector (240, 80);
		Add (radio, 0);

		// THÖ KINKKU
		kinkku = PhysicsObject.CreateStaticObject(Level.Width * 0.25, Level.Height * 0.175);
		kinkku.Image = LoadImage("kinkku");							// 1. Lisätään kinkku
		kinkku.Position = new Vector (-260, -20);
		Add (kinkku, 0);

		elamansuola = new PhysicsObject (Level.Width * 0.05, Level.Height * 0.1);
		elamansuola.Image = LoadImage("elamansuola"); 				// 2. Lisätään suolapurkki
		elamansuola.Position = new Vector (100, -10);
		elamansuola.Tag = "elamansuola";
		ainekset.Add (elamansuola);
		Add (elamansuola, 1);

		hksininen = new PhysicsObject (Level.Width * 0.125, Level.Height * 0.075);
		hksininen.Image = LoadImage("hksininen"); 					// 3. Lisätään HK:n sininen eli makkara
		hksininen.Position = new Vector (380, -60);
		hksininen.Tag = "hksininen";
		ainekset.Add (hksininen);
		Add (hksininen, 1);

		jackdaniels = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.25);
		jackdaniels.Image = LoadImage("jackdaniels"); 				// 4. Lisätään Jack Daniels viskipullo
		jackdaniels.Position = new Vector (-40, 80);
		jackdaniels.Tag = "jackdaniels";
		ainekset.Add (jackdaniels);
		Add (jackdaniels, 1);

		kebabkastike = new PhysicsObject (Level.Width * 0.15, Level.Height * 0.20);
		kebabkastike.Image = LoadImage("kebabkastike"); 			// 5. Lisätään kebabkastikepurkit 
		kebabkastike.Position = new Vector (550, 50);
		kebabkastike.Tag = "kebabkastike";
		ainekset.Add (kebabkastike);
		Add (kebabkastike, 1);

		lanttu = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.1);
		lanttu.Image = LoadImage("lanttu"); 						// 6. Lisätään kolmen lanttua
		lanttu.Position = new Vector (180, -60);
		lanttu.Tag = "lanttu";
		ainekset.Add (lanttu);
		Add (lanttu, 1);

		kossu = new PhysicsObject (Level.Width * 0.065, Level.Height * 0.225);
		kossu.Image = LoadImage("kossu"); 							// 7. Lisätään Koskenkorva viinapullo
		kossu.Position = new Vector (430, 80);
		kossu.Tag = "kossu";
		ainekset.Add (kossu);
		Add (kossu, 1);

		mandariini = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.075);
		mandariini.Image = LoadImage("mandariini"); 				// 8. Lisätään mandariini
		mandariini.Position = new Vector (540, -70);
		mandariini.Tag = "mandariini";
		ainekset.Add (mandariini);
		Add (mandariini, 1);

		marsipaani = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.1);
		marsipaani.Image = LoadImage("marsipaani"); 				// 9. Lisätään marsipaani
		marsipaani.Position = new Vector (70, -70);
		marsipaani.Tag = "marsipaani";
		ainekset.Add (marsipaani);
		Add (marsipaani, 1);

		rakuuna = new PhysicsObject (Level.Width * 0.025, Level.Height * 0.07);
		rakuuna.Image = LoadImage("rakuuna");						// 10. Lisätään rakuuna maustepurkki
		rakuuna.Position = new Vector (60, 0);
		rakuuna.Tag = "rakuuna";
		ainekset.Add (rakuuna);
		Add (rakuuna, 1);

		msmjauhe = new PhysicsObject (Level.Width * 0.05, Level.Height * 0.05);
		msmjauhe.Image = LoadImage("msmjauhe"); 					// 11. Lisätään MSM -jauhe kippo 
		msmjauhe.Position = new Vector (20, -100);
		msmjauhe.Tag = "msmjauhe";
		ainekset.Add (msmjauhe);
		Add (msmjauhe, 1);

		mustaherukka = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.075);
		mustaherukka.Image = LoadImage("mustaherukka"); 			// 12. Lisätään mustaherukat 
		mustaherukka.Position = new Vector (270, -20);
		mustaherukka.Tag = "mustaherukka";
		ainekset.Add (mustaherukka);
		Add (mustaherukka, 1);

		mustakitaturska = new PhysicsObject (Level.Width * 0.25, Level.Height * 0.1);
		mustakitaturska.Image = LoadImage("mustakitaturska"); 		// 13. Lisätään mustakitaturskan
		mustakitaturska.Position = new Vector (240, 165);
		mustakitaturska.Tag = "mustakitaturska";
		ainekset.Add (mustakitaturska);
		Add (mustakitaturska, 1);

		mustapippuri = new PhysicsObject (Level.Width * 0.075, Level.Height * 0.075);
		mustapippuri.Image = LoadImage("mustapippuri"); 			// 14. Lisätään mustapippurit
		mustapippuri.Position = new Vector (-600, -70);
		mustapippuri.Tag = "mustapippuri";
		ainekset.Add (mustapippuri);
		Add (mustapippuri, 1);

		sukkahousut = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.15);
		sukkahousut.Image = LoadImage("sukkahousut"); 				// 15. Lisätään sukkahousut
		sukkahousut.Position = new Vector (-510, -65);
		sukkahousut.Tag = "sukkahousut";
		ainekset.Add (sukkahousut);
		Add (sukkahousut, 1);

		tilli = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.075);
		tilli.Image = LoadImage("tilli"); 							// 16. Lisätään tilli
		tilli.Position = new Vector (380, -105);
		tilli.Tag = "tilli";
		ainekset.Add (tilli);
		Add (tilli, 1);
	}
		
}