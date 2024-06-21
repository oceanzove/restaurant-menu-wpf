
CREATE TABLE Ingredient (
    id INT PRIMARY KEY IDENTITY,
    [name] VARCHAR(255) NOT NULL,
    protein DECIMAL(5,2) NOT NULL,
    fat DECIMAL(5,2) NOT NULL,
    carbohydrate DECIMAL(5,2) NOT NULL
);

CREATE TABLE Menu (
    id INT PRIMARY KEY IDENTITY,
    [name] VARCHAR(255) NOT NULL,
    approved BIT NOT NULL
);

CREATE TABLE MenuIngredient (
    menu_id INT REFERENCES menu(id),
    ingredient_id INT REFERENCES ingredient(id),
    [weight] DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (menu_id, ingredient_id),
);