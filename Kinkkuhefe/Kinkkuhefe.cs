using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Kinkkuhefe : PhysicsGame
{
	// TAUSTAKUVAT
	Image aloitusValikko = LoadImage("aloitusValikko"); 	// Ladataan keittiöstä kuva alkuvalikon taustaksi.
	Image pelinTausta = LoadImage("pelinTausta"); 			// Ladataan todellisen toiminnan aikainen näkymä.

	// ALKUVALIKON KOHDAT LISTA
	List<Label> valikonKohdat;

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


	// LUODAAN ALKUVALIKKO 
	void alkuValikko()
	{
		ClearAll (); 													// Tyhjennetään kenttä kaikista peliolioista
		valikonKohdat = new List<Label> (); 							// Alustetaan lista, johon valikon kohdat tulevat
		Level.Background.Image = aloitusValikko; 						// Ladataan keittiöstä kuva pelin aloitusruuduksi

		Label kohta1 = new Label ("Aloita uuden kinkun maustaminen.");  // Luodaan uusi Label-olio, joka toimii uuden pelin aloituskohtana
		kohta1.Position = new Vector (0, 40);  							// Asetetaan valikon ensimmäinen kohta hieman kentän keskikohdan yläpuolelle
		valikonKohdat.Add (kohta1);  									// Lisätään luotu valikon kohta listaan jossa kohtia säilytetään

		// Lisätään kaikki luodut kohdat peliin foreach-silmukalla
		foreach (Label valikonKohta in valikonKohdat) 
		{
			Add (valikonKohta);
		}
	}


	// PÄÄOHJELMA
	public override void Begin ()
	{
		MultiSelectWindow alkuValikko = new MultiSelectWindow("Oletko kova paistamaan kinkkua?", "OTETAAN!", "HALL OF KINKKUHEFE", "Syön mieluummin ananaspizzaa.");
		alkuValikko.DefaultCancel = 3;							// Peruutusnäppäimellä pelistä pihalle eli ESC poistuu pelistä kuten "Syön mieluummin ananaspizzaa"
		Level.Background.Image = pelinTausta; 					// Sitten tämä kun aletaan kunnolla kokkaamaan!
		Add(alkuValikko);

		IsFullScreen = true; 									// Peli asetetaan kokonäytölle.			
		Mouse.IsCursorVisible = true; 							// Hiiri näkyviin.

		// MAUSTEET / AINEKSET OBJEKTEIKSI
		// THÖ KINKKU
		kinkku = PhysicsObject.CreateStaticObject(Level.Width * 0.3, Level.Height * 0.2);
		kinkku.Image = LoadImage("kinkku");							// 1. Lisätään kinkku
		kinkku.X = -250;
		kinkku.Y = -20;
		Add (kinkku);

		elamansuola = new PhysicsObject (Level.Width * 0.05, Level.Height * 0.1);
		elamansuola.Image = LoadImage("elamansuola"); 				// 2. Lisätään suolapurkki
		elamansuola.X = 100;
		elamansuola.Y = 100;
		elamansuola.Tag = "aines";
		Add (elamansuola);

		hksininen = new PhysicsObject (Level.Width * 0.3, Level.Height * 0.1);
		hksininen.Image = LoadImage("hksininen"); 					// 3. Lisätään HK:n sininen eli makkara
		hksininen.X = 150;
		hksininen.Y = 100;
		hksininen.Tag = "aines";
		Add (hksininen);


		jackdaniels = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.3);
		jackdaniels.Image = LoadImage("jackdaniels"); 				// 4. Lisätään Jack Daniels viskipullo
		jackdaniels.X = 200;
		jackdaniels.Y = 100;
		jackdaniels.Tag = "aines";
		Add (jackdaniels);

		kebabkastike = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.3);
		kebabkastike.Image = LoadImage("kebabkastike"); 			// 5. Lisätään kebabkastikepurkit 
		kebabkastike.X = 300;
		kebabkastike.Y = 100;
		kebabkastike.Tag = "aines";
		Add (kebabkastike);

		lanttu = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		lanttu.Image = LoadImage("lanttu"); 						// 6. Lisätään kolmen lanttua
		lanttu.X = 400;
		lanttu.Y = 100;
		lanttu.Tag = "aines";
		Add (lanttu);

		kossu = new PhysicsObject (Level.Width * 0.1, Level.Height * 0.3);
		kossu.Image = LoadImage("kossu"); 							// 7. Lisätään Koskenkorva viinapullo
		kossu.X = 500;
		kossu.Y = 100;
		kossu.Tag = "aines";
		Add (kossu);

		mandariini = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		mandariini.Image = LoadImage("mandariini"); 				// 8. Lisätään mandariini
		mandariini.X = 600;
		mandariini.Y = 100;
		mandariini.Tag = "aines";
		Add (mandariini);

		marsipaani = new PhysicsObject (Level.Width * 0.4, Level.Height * 0.2);
		marsipaani.Image = LoadImage("marsipaani"); 				// 9. Lisätään marsipaani
		marsipaani.X = 700;
		marsipaani.Y = 100;
		marsipaani.Tag = "aines";
		Add (marsipaani);

		rakuuna = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		rakuuna.Image = LoadImage("rakuuna");						// 10. Lisätään rakuuna maustepurkki
		rakuuna.X = -100;
		rakuuna.Y = 100;
		rakuuna.Tag = "aines";
		Add (rakuuna);

		msmjauhe = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		msmjauhe.Image = LoadImage("msmjauhe"); 					// 11. Lisätään MSM -jauhe kippo 
		msmjauhe.X = -200;
		msmjauhe.Y = 100;
		msmjauhe.Tag = "aines";
		Add (msmjauhe);

		mustaherukka = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		mustaherukka.Image = LoadImage("mustaherukka"); 			// 12. Lisätään mustaherukat 
		mustaherukka.X = -300;
		mustaherukka.Y = 100;
		mustaherukka.Tag = "aines";
		Add (mustaherukka);

		mustakitaturska = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		mustakitaturska.Image = LoadImage("mustakitaturska"); 		// 13. Lisätään mustakitaturskan
		mustakitaturska.X = -400;
		mustakitaturska.Y = 100;
		mustakitaturska.Tag = "aines";
		Add (mustakitaturska);

		mustapippuri = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		mustapippuri.Image = LoadImage("mustapippuri"); 			// 14. Lisätään mustapippurit
		mustapippuri.X = -500;
		mustapippuri.Y = 100;
		mustapippuri.Tag = "aines";
		Add (mustapippuri);

		sukkahousut = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		sukkahousut.Image = LoadImage("sukkahousut"); 				// 15. Lisätään sukkahousut
		sukkahousut.X = -600;
		sukkahousut.Y = 100;
		sukkahousut.Tag = "aines";
		Add (sukkahousut);

		tilli = new PhysicsObject (Level.Width * 0.2, Level.Height * 0.2);
		tilli.Image = LoadImage("tilli"); 							// 16. Lisätään tilli
		tilli.X = -700;
		tilli.Y = 100;
		tilli.Tag = "aines";
		Add (tilli);

		/*
		// OHJEKENTTÄ PELAAJALLE
		Label tekstikentta = new Label(300, 50, "ALAHAN KOKATA POIKA!");
		tekstikentta.X = Screen.Right - 250;
		tekstikentta.Y = Screen.Top - 50;
		tekstikentta.TextColor = Color.DarkGray;
		tekstikentta.BorderColor = Color.Black;
		Add(tekstikentta);
		*/


		// HIIREN KÄYTTÖ OBJEKTIEN LIIKUTTELUUN & TUTKIMISEEN
		//if(Mouse.
		Mouse.Listen (MouseButton.Left, ButtonState.Pressed, KuunteleLiiketta2, "Jos ei koordinaatio riitä ;D");
		Mouse.Listen (MouseButton.Left, ButtonState.Down, KuunteleLiiketta, "Lisää aineksia kinkkuun mausteeksi.");


		// PELIN LOPETTAMINEN
		PhoneBackButton.Listen (ConfirmExit, "Lopeta peli");
		Keyboard.Listen (Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
	}


	// HIIREN KUUNTELU ELI MITÄ TAPAHTUU KUN VASEMMALLA HIIRELLÄ VAAN KLIKATAAN
	void KuunteleLiiketta2()
	{
		int k = 1;
		MessageDisplay.Clear();							// Tyhjennetään tekstiruutu edellisestä viisastelusta.

		while (k == 1) 
		{
			MessageDisplay.Add ("TARTU KUIN MIES!");
			MessageDisplay.MaxMessageCount = 0;
		}
	}


	// HIIREN KUUNTELU ELI MITÄ TAPAHTUU KUN VASEN HIIRI ON PAINETTU POHJAAN
	void KuunteleLiiketta()
	{   
		int k = 1;
		MessageDisplay.Clear();							// Tyhjennetään tekstiruutu edellisestä viisastelusta.

		while(k == 1)
		{
			if (Mouse.IsCursorOn(elamansuola)) 
			{
				elamansuola.X = Mouse.PositionOnWorld.X;
				elamansuola.Y = Mouse.PositionOnWorld.Y;

				MessageDisplay.Add( "Käytä ensi kerralla Himalajan suolaa" );
				MessageDisplay.MaxMessageCount = 0;
				k++;
			}
				
			else if (Mouse.IsCursorOn(hksininen)) 
			{
				hksininen.X = Mouse.PositionOnWorld.X;
				hksininen.Y = Mouse.PositionOnWorld.Y;

				MessageDisplay.Add( "Vähä kyrsää... Brus suomalaista!" );
				MessageDisplay.MaxMessageCount = 0;
				k++;
			}

			else if (Mouse.IsCursorOn(jackdaniels)) 
			{
				jackdaniels.X = Mouse.PositionOnWorld.X;
				jackdaniels.Y = Mouse.PositionOnWorld.Y;

				MessageDisplay.Add( "Sullahan on ihan kehittynyt maku." );
				MessageDisplay.MaxMessageCount = 0;
				k++;
			}

			else if (Mouse.IsCursorOn(lanttu)) 
			{
				lanttu.X = Mouse.PositionOnWorld.X;
				lanttu.Y = Mouse.PositionOnWorld.Y;

				MessageDisplay.Add( "Meidän kunnioitettu puheenjohtaja." );
				MessageDisplay.MaxMessageCount = 0;
				k++;
			} 

			else if (Mouse.IsCursorOn(kossu)) 
			{
				kossu.X = Mouse.PositionOnWorld.X;
				kossu.Y = Mouse.PositionOnWorld.Y;

				MessageDisplay.Add( "Vähä kyrsää... Brus suomalaista!" );
				MessageDisplay.MaxMessageCount = 0;
				k++;
			}
		}
	}


}