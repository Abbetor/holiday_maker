USE holiday_maker;
CREATE TABLE Boende (
    id INT AUTO_INCREMENT PRIMARY KEY,
    namn VARCHAR(100) NOT NULL,
    ort VARCHAR(100) NOT NULL,
    pool BOOLEAN NOT NULL,
    restaurang BOOLEAN NOT NULL,
    avstand_strand INT NOT NULL,
    avstand_centrum INT NOT NULL,
    omdome DECIMAL(2,1) NOT NULL,
    CHECK (omdome BETWEEN 1 AND 5)
);

CREATE TABLE Rum (
    id INT AUTO_INCREMENT PRIMARY KEY,
    rumsnummer INT NOT NULL,
    max_personer INT NOT NULL,
    pris_per_natt DECIMAL(10,2) NOT NULL,
    boende_id INT NOT NULL,

    UNIQUE (boende_id, rumsnummer),

    CHECK (max_personer > 0),

    FOREIGN KEY (boende_id) REFERENCES Boende(id)
);

CREATE TABLE Kund (
    id INT AUTO_INCREMENT PRIMARY KEY,
    namn VARCHAR(100) NOT NULL,
    efternamn VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL,
    telefon VARCHAR(30) NOT NULL
);

CREATE TABLE Bokning (
    id INT AUTO_INCREMENT PRIMARY KEY,
    antal_personer INT NOT NULL,
    kund_id INT NOT NULL,
    rum_id INT NOT NULL,
    incheckning DATE NOT NULL,
    utcheckning DATE NOT NULL,

    FOREIGN KEY (kund_id) REFERENCES Kund(id),
    FOREIGN KEY (rum_id) REFERENCES Rum(id),

    CHECK (antal_personer > 0),
    CHECK (utcheckning > incheckning)
);

INSERT INTO Boende (
    namn,
    ort,
    pool,
    restaurang,
    avstand_strand,
    avstand_centrum,
    omdome
)
VALUES
    ('Hotel Ocean', 'Ystad', TRUE, TRUE, 200, 500, 4.5),
    ('Sun Hotel', 'Malmö', TRUE, FALSE, 800, 1200, 3.8),
    ('Grand Inn', 'Malmö', FALSE, TRUE, 1500, 300, 4.2),
    ('Beach Resort', 'Ystad', TRUE, TRUE, 100, 700, 4.9),
    ('City Lodge', 'Lund', FALSE, FALSE, 5000, 400, 3.5);

INSERT INTO Rum (
    rumsnummer,
    max_personer,
    pris_per_natt,
    boende_id
)
VALUES
    (1, 1, 900, 1),
    (2, 2, 1200, 1),
    (3, 4, 1800, 1),
    (4, 2, 1400, 1),

    (1, 1, 800, 2),
    (2, 2, 1100, 2),
    (3, 4, 1700, 2),
    (4, 4, 1900, 2),

    (1, 2, 1000, 3),
    (2, 2, 1300, 3),
    (3, 4, 2000, 3),
    (4, 1, 750, 3),

    (1, 1, 950, 4),
    (2, 2, 1250, 4),
    (3, 4, 2100, 4),
    (4, 4, 2300, 4),

    (1, 1, 700, 5),
    (2, 2, 1000, 5),
    (3, 2, 1150, 5),
    (4, 4, 1600, 5);


INSERT INTO Kund (namn,
                  efternamn,
                  email,
                  telefon)
VALUES ('Erik', 'Andersson', 'erik@example.com', '070-1111111'),
       ('Anna', 'Karlsson', 'anna@example.com', '070-2222222'),
       ('Johan', 'Nilsson', 'johan@example.com', '070-3333333'),
       ('Sara', 'Svensson', 'sara@example.com', '070-4444444'),
       ('Lisa', 'Larsson', 'lisa@example.com', '070-5555555');

INSERT INTO Bokning (
    antal_personer,
    kund_id,
    rum_id,
    incheckning,
    utcheckning
)
VALUES
    -- Erik
    (1, 1, 1,  '2026-12-10', '2026-12-13'),
    (2, 1, 2,  '2026-12-15', '2026-12-18'),
    (4, 1, 3,  '2026-12-20', '2026-12-24'),
    (2, 1, 7,  '2026-12-05', '2026-12-08'),
    (1, 1, 13, '2026-12-01', '2026-12-04'),

    -- Anna
    (2, 2, 5,  '2026-12-10', '2026-12-14'),
    (4, 2, 8,  '2026-12-18', '2026-12-21'),
    (2, 2, 9,  '2026-12-22', '2026-12-25'),
    (1, 2, 12, '2026-12-05', '2026-12-07'),
    (2, 2, 14, '2026-12-27', '2026-12-30'),

    -- Johan
    (2, 3, 2,  '2026-12-02', '2026-12-05'),
    (4, 3, 4,  '2026-12-12', '2026-12-15'),
    (4, 3, 11, '2026-12-20', '2026-12-23'),
    (1, 3, 17, '2026-12-06', '2026-12-09'),
    (2, 3, 18, '2026-12-15', '2026-12-18'),

    -- Sara
    (4, 4, 3,  '2026-12-01', '2026-12-04'),
    (2, 4, 6,  '2026-12-10', '2026-12-13'),
    (4, 4, 15, '2026-12-18', '2026-12-22'),
    (2, 4, 19, '2026-12-24', '2026-12-27'),
    (1, 4, 20, '2026-12-30', '2027-01-02'),

    -- Lisa
    (2, 5, 10, '2026-12-03', '2026-12-06'),
    (1, 5, 16, '2026-12-08', '2026-12-11'),
    (4, 5, 8,  '2026-12-15', '2026-12-18'),
    (2, 5, 14, '2026-12-20', '2026-12-23'),
    (4, 5, 20, '2026-12-26', '2026-12-30');
