const express = require('express');
const router = express.Router();
const db = require('../db');

class PipeController {
    async getOne(req, res) {
        const id = req.params.id; // Получаем id от клиента
        const pipe = await db.query(
            `SELECT 
                p.id_tube, 
                a.num_auditorium,   
                p.id_rizer, 
                p.length, 
                p.diameter, 
                p.heating_area,
                p.material,
                p.coefficient, 
                p.temperature,
                t.type AS type_name
            FROM tube p
            JOIN auditorium a ON p.id_auditorium = a.id_auditorium
            LEFT JOIN temperature_teplonositelya t ON p.type = t.id_type
            WHERE p.id_tube = $1;`, [id]);

        if (!pipe.rows.length) {
            res.status(404).json({ message: 'Pipe not found' });
        } else {
            const result = pipe.rows[0];
            res.json({
                "id_tube": result.id_tube,
                "num_auditorium": result.num_auditorium,
                "id_rizer": result.id_rizer,
                "length": result.length,
                "diameter": result.diameter,
                "material": result.material,
                "temperature": result.temperature,
                "type_name": result.type_name,
            });
        }
    }
}

const pipeController = new PipeController();

router.get('/pipe/:id', pipeController.getOne);

module.exports = router;