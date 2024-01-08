
CREATE TABLE items (
	id serial PRIMARY KEY,
	key varchar(255) NOT NULL,
	content text,
	expires_at timestamp DEFAULT NOW(),
	expiration_period int
);

CREATE INDEX idx_items_key ON items (key);

