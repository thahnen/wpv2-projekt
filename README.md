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
    "Width" : 3,
    "Height" : 2,
    "Nodes" : [
        {"Id" : 1, "Coords" : {"uX" : 0, "uY" : 0}},
        {"Id" : 2, "Coords" : {"uX" : 1, "uY" : -1}},
        {"Id" : 3, "Coords" : {"uX" : 1, "uY" : 1}},
        {"Id" : 4, "Coords" : {"uX" : 2, "uY" : 0}}
    ],
    "Adjacency" : [
        [false, true, true, false],
        [false, false, false, true],
        [false, false, false, true],
        [false, false, false, false]
    ]
}
```

Dabei gibt "Width" die Breite des Graphen an, "Height" die maximale Höhe.
"Nodes" gibt nach wie vor die Knoten an, allerdings diesmal mit universellen X- und Y-Koordinaten und der Id nach sortiert.
Anstelle der Liste der Kanten gibt es nun eine Adjazenzmatrix, die Zeile gibt Start an, Spalte das Ziel der Kante.
Dabei ist der gegebene Index in der Liste gleich dem Index in "Nodes".

---

## TODO:
* Pfeile zeichnen von A -> B
* Richtigere Anordnung der Knoten
