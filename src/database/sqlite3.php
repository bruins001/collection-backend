<?php
class Sqlite3Database {
    private SQLite3 $db;

    public function __construct() {
        // Creates an database if it doesn't exist and opens it otherwise.
        // Throws Exception on failure.
        $this->db = new SQLite3('sqlDb.db');
    }
}
?>