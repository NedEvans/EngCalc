-- Delete all test data to allow reseeding with AppConstant bindings

-- Delete in correct order due to foreign keys
DELETE FROM CardInstances;
DELETE FROM CalculationRevisions;
DELETE FROM Calculations;
DELETE FROM GlobalConstants;
DELETE FROM Jobs;
DELETE FROM Projects;
DELETE FROM Cards;

PRINT 'Test data deleted. Restart the application to reseed with AppConstant bindings.';
