# Projekt für Anwendungsentwicklung in C# 
**Vorlesung Anwendungsentwicklung in C#**

Beinhaltet das Projekt als Vorleistung zur mündlichen Prüfung.
Läd einen pseudo-Graphen aus einer JSON-Datei, fügt den Knoten und Kanten uniforme Koordinaten hinzu und speichert den entstandenen Graphen in einer weiteren JSON-Datei.

---

## Technik
* C#
* Windows Presentation Foundation (WPF)
* [JSON.NET für C#](https://www.newtonsoft.com/json)

---

## Examples

### Input
Die Eingabe muss wie folgt aussehen:
```json
{
    "Nodes" : [
        {"id_" : 1},
        {"id_" : 2},
        {"id_" : 3},
        {"id_" : 4}
    ],
    "Edges" : [
        [1, 2], [1, 3],
        [2, 4], [3, 4]
    ]
}
```

Dabei gibt "Nodes" die Knoten im Graphen an und "Edges" eine Liste von Kanten (Start-Id, Ziel-Id).

### Output
Ergebnis ist nach der Erstellung von universellen Koordinaten:
```json
{
    "width" : 3,
    "height" : 2,
    "nodes" : [
        {"id" : 1, "coords" : {"ux" : 0, "uy" : 0}},
        {"id" : 2, "coords" : {"ux" : 1, "uy" : -1}},
        {"id" : 3, "coords" : {"ux" : 1, "uy" : 1}},
        {"id" : 4, "coords" : {"ux" : 2, "uy" : 0}}
    ],
    "adjacency" : [
        [false, true, true, false],
        [false, false, false, true],
        [false, false, false, true],
        [false, false, false, false]
    ]
}
```

Dabei gibt "width" die Breite des Graphen an, "height" die Höhe.
"nodes" nach wie vor die Knoten, allerdings diesmal mit universellen X- und Y-Koordinaten und der Id nach sortiert.
Anstelle der Liste der Kanten gibt es nun eine Adjazenzmatrix, die Zeile gibt Start an, Spalte das Ziel der Kante.
Dabei ist der gegebene Index in der Liste gleich dem Index in "nodes".

---

## TODO:
* Pfeile zeichnen von A -> B
