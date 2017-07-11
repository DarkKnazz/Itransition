class CreateRooms < ActiveRecord::Migration[5.0]
  def change
    create_table :rooms do |t|
      t.text :content
      t.integer :users_id
      t.string :name

      t.timestamps
    end
  end
end
