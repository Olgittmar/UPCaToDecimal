# UPCaToDecimal
____Arbetsprov för jobbansökan till Sectra.____

Konsoll-applikation som tar en textfil [UTF8 No BOM] med 1 UPC-A kod per rad,
och skriver motsvarande decimala kod antingen direkt till Konsollen,en textfil om sådan angetts med `-o` argumentet
eller båda om argumentet `-c` anges tillsammans med utfil.

Ex.
Om `UPCaTextFil.txt` innehåller:   
```▍ ▍   ▍▍ ▍ ▍▍   ▍  ▍▍  ▍   ▍▍ ▍   ▍▍ ▍   ▍▍ ▍ ▍ ▍ ▍▍▍  ▍ ▍▍  ▍▍ ▍▍ ▍▍  ▍  ▍▍▍ ▍▍  ▍▍ ▍   ▍  ▍ ▍```   
   
Så resulterar ```UPCaToDecimal UPCATextFil.txt -o ResultatFil.txt -c``` I att `0 51000 01251 7`
skrivs till både `ResultatFil.txt` och konsollen eftersom argumentet `-c` angetts.


____Programming test for job application with Sectra.____

Console application which takes a text file [UTF8 No BOM] which contains 1 UPC-A code per row,
and writes the corresponding decimal code either directly to the console, a textfile if one has been provided with the `-o` argument
or both if the argument `-c` was provided alongside an output file.

Ex.
If ```UPCaSourceFile.txt``` contains:   
```▍ ▍   ▍▍ ▍ ▍▍   ▍  ▍▍  ▍   ▍▍ ▍   ▍▍ ▍   ▍▍ ▍ ▍ ▍ ▍▍▍  ▍ ▍▍  ▍▍ ▍▍ ▍▍  ▍  ▍▍▍ ▍▍  ▍▍ ▍   ▍  ▍ ▍```   
   
Then ```UPCaToDecimal UPCaSourceFile.txt -o ResultFile.txt -c```
writes ```0 51000 01251 7``` to both ```ResultFile.txt``` and the console, since the `-c` argument was provided.
