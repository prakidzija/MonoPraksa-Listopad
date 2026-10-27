-- Run once against an existing VolleyballApp database before using the updated API.
-- Requires permission to install btree_gist. No registration rows are changed/deleted.
-- End dates are inclusive; NULL means unbounded. Conflicting legacy data aborts
-- this transaction: resolve it explicitly, then rerun. Do not rerun PraksaDB.sql.
BEGIN;

CREATE EXTENSION IF NOT EXISTS btree_gist;
LOCK TABLE player_registrations IN ACCESS EXCLUSIVE MODE;

DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM player_registrations WHERE jersey_number NOT BETWEEN 1 AND 99) THEN
        RAISE EXCEPTION 'Existing jersey numbers outside 1-99 must be corrected before migration.';
    END IF;
END $$;

ALTER TABLE player_registrations
    DROP CONSTRAINT player_registrations_club_id_jersey_number_key,
    ADD CONSTRAINT player_registrations_jersey_number_check
        CHECK (jersey_number BETWEEN 1 AND 99),
    ADD CONSTRAINT player_registrations_jersey_dates_excl
        EXCLUDE USING gist (
            club_id WITH =,
            jersey_number WITH =,
            daterange(start_date, end_date, '[]') WITH &&
        );

COMMIT;
