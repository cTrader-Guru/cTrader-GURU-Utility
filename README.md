# cTrader-GURU-Utility
Una libreria open-source con tutte le utility più utilizzate e avanzate per [cTrader](https://buff.ly/2G7fnkN)

Ecco un esempio di come utilizzarla, una volta clonata o scaricata la soluzione compilatela, oppure nella cartella **DLL-COMPILED** nella radice troverete l'ultima versione stabile compilata personalmente quindi sicura.

Nel vostro cBot create un riferimento a tale libreria **cTrader-GURU.dll** , poi scrivete quanto segue :

    using System;  
    using cAlgo.API;  
    using cTrader_GURU;  
      
    namespace cAlgo.Robots  
    {  
	    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]  
	    public class TestDLL : Robot  
	    {  
	      
		    protected override void OnStart()  
		    {  
		      
			    GURU hello = new GURU();  
			      
			    Print(hello.ciao());  
			      
			    Print(GURUs.Ciao());  
		      
		    }  
	      
	    }  
      
    }

Compilatelo ed eseguitelo in una istanza, nel log del cBot leggerete **Hello World !** e **Hello Worlds !**
trovate tutte le spiegazioni nel [wiki](https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki)

Licenza MIT, sapete cosa fare 😉
