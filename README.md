# UPCaToDecimal
Arbetsprov för jobbansökan till Sectra

Konsoll-applikation som tar en textfil [UTF8 No BOM] med 1 UPC-A kod per rad,
och skriver motsvarande decimala kod antingen till Konsollen eller en textfil om sådan angetts.

Ex. UPCaToDecimal UPCATextFil.txt -o ResultatFil.txt -c
Skriver resultatet till "ResultatFil.txt" och konsollen eftersom argumentet "-c" angetts.
