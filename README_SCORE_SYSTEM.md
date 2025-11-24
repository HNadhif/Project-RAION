# Project RAION - Score & Shooting System

Dokumentasi bagian **Score & Shooting** untuk tim development.

## 🎯 Yang Sudah Diimplementasi

### Features:
- ✅ **Player Shooting**: Spacebar untuk menembak
- ✅ **Score System**: +100 poin per enemy yang dihancurkan  
- ✅ **Game Over**: Collision detection + restart

### Script yang Dibuat/Dimodifikasi:
```
Assets/Scripts/Day 2/
├── ScoreManager.cs      # ⭐ BARU - Sistem score
├── Movements.cs         # ✏️ MODIFIED - Tambah shooting ke movement
├── Bullet.cs            # ✏️ MODIFIED - Collision detection
├── Enemy.cs             # ✏️ MODIFIED - Collision dengan player bullet
└── Restart.cs           # ✏️ MODIFIED - Reset score saat restart
```

## 🔧 Setup untuk Tim

### Yang Harus Dilakukan di Unity:

#### 1. Player GameObject:
- **Attach script `Movements`** (sudah include shooting)
- **Set di Inspector:**
  - Bullet Prefab: `bulletPlayer.prefab`
  - Fire Point: (optional - empty GameObject di ujung pesawat)
  - Game Over Canvas: (drag canvas game over kalian)

#### 2. Score Manager:
- **Buat GameObject "ScoreManager"** + attach script `ScoreManager`
- **Buat UI Text** untuk score → drag ke field "Score Text" 

#### 3. Tags yang Diperlukan:
```
Player       → Player GameObject
Enemy        → Enemy prefab  
PlayerBullet → bulletPlayer.prefab
EnemyBullet  → bulletEnemy.prefab (kalau ada)
```

