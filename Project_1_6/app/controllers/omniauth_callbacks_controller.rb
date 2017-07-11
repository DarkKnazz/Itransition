
class OmniauthCallbacksController < Devise::OmniauthCallbacksController
  def facebook
	@user = User.facebook_from_omniauth(request.env["omniauth.auth"])
	sign_in_and_redirect @user
  end

  def vkontakte
	@user = User.vkontakte_from_omniauth(request.env["omniauth.auth"])
	sign_in_and_redirect @user
  end

  def twitter
	@user = User.twitter_from_omniauth(request.env["omniauth.auth"])
	sign_in_and_redirect @user
  end
end