-- Run after the reusable-jersey migration, e.g. with psql -v ON_ERROR_STOP=1 -f ...
-- Uses a temporary copy with the real check/exclusion constraints and no foreign keys.
-- No production rows are inserted or updated; all test work is rolled back.
BEGIN;
CREATE TEMP TABLE jersey_rule_test (LIKE player_registrations INCLUDING ALL) ON COMMIT DROP;

DO $$
DECLARE
    club_a uuid := gen_random_uuid();
    club_b uuid := gen_random_uuid();
    player uuid := gen_random_uuid();
    rejected boolean;
    invalid_number integer;
BEGIN
    INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
    VALUES (player, club_a, 'permanent', 1, '2026-01-01', '2026-06-30');

    -- Reuse after expiry succeeds, including an open-ended new registration.
    INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
    VALUES (player, club_a, 'loan', 1, '2026-07-01', NULL);

    -- Earlier non-overlapping dates, other clubs, and the upper boundary succeed.
    INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
    VALUES (player, club_a, 'loan', 1, '2025-01-01', '2025-12-31'),
           (player, club_b, 'permanent', 1, '2026-01-01', NULL),
           (player, club_a, 'permanent', 99, '2026-01-01', NULL);

    rejected := false;
    BEGIN
        -- End dates are inclusive: reusing on the last day still overlaps.
        INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
        VALUES (player, club_a, 'loan', 1, '2026-06-30', '2026-06-30');
    EXCEPTION WHEN exclusion_violation THEN
        rejected := true;
    END;
    IF NOT rejected THEN RAISE EXCEPTION 'Expected same-day overlap to fail'; END IF;

    rejected := false;
    BEGIN
        INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
        VALUES (player, club_a, 'loan', 1, '2030-01-01', '2030-12-31');
    EXCEPTION WHEN exclusion_violation THEN
        rejected := true;
    END;
    IF NOT rejected THEN RAISE EXCEPTION 'Expected overlap with open-ended registration to fail'; END IF;

    rejected := false;
    BEGIN
        -- A containing interval must conflict too, not just matching start dates.
        INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
        VALUES (player, club_a, 'loan', 1, '2024-01-01', NULL);
    EXCEPTION WHEN exclusion_violation THEN
        rejected := true;
    END;
    IF NOT rejected THEN RAISE EXCEPTION 'Expected containing interval to fail'; END IF;

    FOREACH invalid_number IN ARRAY ARRAY[-1, 0, 100] LOOP
        rejected := false;
        BEGIN
            INSERT INTO jersey_rule_test (player_id, club_id, registration_type, jersey_number, start_date, end_date)
            VALUES (player, club_a, 'loan', invalid_number, '2026-01-01', NULL);
        EXCEPTION WHEN check_violation THEN
            rejected := true;
        END;
        IF NOT rejected THEN RAISE EXCEPTION 'Expected jersey % to fail', invalid_number; END IF;
    END LOOP;
END $$;

ROLLBACK;
