json.extract! room, :id, :content, :users_id, :name, :created_at, :updated_at
json.url room_url(room, format: :json)
