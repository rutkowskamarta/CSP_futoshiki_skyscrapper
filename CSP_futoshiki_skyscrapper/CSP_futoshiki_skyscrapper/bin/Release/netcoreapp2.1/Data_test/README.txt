Standard plików Futoshiki (przykład dla N=4):

N
START:
4;0;0;0
0;1;0;4
0;0;2;0
0;3;0;0
REL:
B2;B3
D2;C2

W pierwszej linijce znajduje się liczba N informująca o rozmiarze łamigłówki.
Następnie pod linijką z napisem 'START:' znajduje się reprezentacja stanu początkowego planszy. W odpowiednich miejscach znajdują się wartości liczbowe od 1 do N. Miejsca te przyjmujemy za wypełnione. Jeżeli na miejscu znajduje się wartość 0 oznacza to, że pole jest puste i należy je wypełnić podczas rozwiązywania łamigłówki.
Następną sekcją są linijki po linii "REL:", która informuje nas o relacjach między polami. WAŻNE! Każda zapisana tam relacja oznacza, że pierwsze (lewe) pole z pary jest mniejsze od drugiego (prawego) pola z pary. Rzędy oznaczane są kolejnymi literami alfabetu łacińskiego (począwszy od A), a kolumny kolejnymi liczbami naturalnymi (począwszy od 1).

Standarad plików Skyscrapper (przykład dla N=4):

N
G;1;3;2;3
D;0;2;0;1
L;1;0;0;4
P;2;2;2;2

W pierwszej linijce znajduje się liczba N informująca o rozmiarze łamigłówki.
Plik zawiera informację o krawędziach planszy odpowiednio [G]órna, [D]olna, [L]ewa, [P]rawa. W opisie krawędzi znajdują się liczby od 1 do N. Jeżeli na miejscu znajduje się wartość 0 oznacza to, że pole nie posiada informacji ile budynków z powinno się z niego widzieć. Pola uzupełniamy z lewej do prawej dla krawędzi G i D, oraz z góry do doły dla krawędzi L i P.