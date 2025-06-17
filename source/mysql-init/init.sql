CREATE DATABASE IF NOT EXISTS ordermaker_identity;
CREATE DATABASE IF NOT EXISTS ordermaker_database;
CREATE DATABASE IF NOT EXISTS ordermaker_inbox;

-- Создаем пользователя, если не существует
DROP USER IF EXISTS 'ordermaker'@'%';
CREATE USER 'ordermaker'@'%' IDENTIFIED WITH mysql_native_password BY 'password';

GRANT ALL PRIVILEGES ON ordermaker_identity.* TO 'ordermaker'@'%';
GRANT ALL PRIVILEGES ON ordermaker_database.* TO 'ordermaker'@'%';
GRANT ALL PRIVILEGES ON ordermaker_inbox.* TO 'ordermaker'@'%';
FLUSH PRIVILEGES;

