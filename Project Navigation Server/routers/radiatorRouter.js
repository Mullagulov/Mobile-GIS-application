const express = require('express');
const router = express.Router();
const db = require('../db');

class RadiatorController {
  async getOne(req, res) {
    const id = req.params.id; // Получаем id от клиента
    const radiator = await db.query(
      `SELECT 
        r.id_radiator, 
        a.num_auditorium,   
        r.id_riser, 
        r.type, 
        r.floor, 
        r.model, 
        r.quantity_section, 
        r.power, 
        r.data_begin, 
        r.data_end, 
        r.material 
      FROM radiator r 
      JOIN auditorium a 
      ON r.id_auditorium = a.id_auditorium 
      WHERE r.id_radiator = $1;`, [id]);

    const result = radiator.rows[0];

    res.json({
        "num_auditorium": result.num_auditorium,
        "id_riser": result.id_riser,
        "type": result.type,
        "floor": result.floor,
        "model": result.model,
        "quantity_section": result.quantity_section,
        "power": result.power,
        "data_begin": result.data_begin,
        "data_end": result.data_end,
        "material": result.material,
    })
  }
}

const radiatorController = new RadiatorController();

router.get('/radiator/:id', radiatorController.getOne);

module.exports = router;